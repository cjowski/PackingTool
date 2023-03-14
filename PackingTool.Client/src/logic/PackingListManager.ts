import type { Ref } from "vue"
import router from "@/router"
import type { PackingItemType } from "src/api/models/PackingItemType"
import type { PackingItemAttribute } from "src/api/models/PackingItemAttribute"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingGroup } from "@/models/packing/list/PackingGroup"
import { PackingListState } from "@/models/packing/list/PackingListState"
import { PackingList } from "@/models/packing/list/PackingList"
import { PackingItem } from "@/models/packing/list/PackingItem"
import { PackingListService } from "@/api"
import { useAuthenticationStore } from "@/stores/authenticationStore"

export class PackingListManager {
  private _allPackingLists: Ref<PackingList[]>
  private _selectedPackingList: Ref<PackingList>
  private _selectedListName: Ref<string> //TODO verify if needed
  private _allListsFetched: Ref<boolean>
  private _currentAction: Ref<PackingListAction>
  private _previousAction: Ref<PackingListAction>
  private _copiedGroups: Ref<PackingGroup[]>
  private _copiedItems: Ref<PackingItem[]>

  constructor(
    allPackingLists: Ref<PackingList[]>,
    packingList: Ref<PackingList>,
    selectedListName: Ref<string>,
    allListsFetched: Ref<boolean>,
    currentAction: Ref<PackingListAction>,
    previousAction: Ref<PackingListAction>,
    copiedGroups: Ref<PackingGroup[]>,
    copiedItems: Ref<PackingItem[]>
  ) {
    this._allPackingLists = allPackingLists
    this._selectedPackingList = packingList
    this._selectedListName = selectedListName
    this._allListsFetched = allListsFetched
    this._currentAction = currentAction
    this._previousAction = previousAction
    this._copiedGroups = copiedGroups
    this._copiedItems = copiedItems
  }

  GetUserID = () => {
    const { userID } = useAuthenticationStore()
    return userID()
  }

  AnyActionInProgress = () => {
    return this._currentAction.value != PackingListAction.None
  }

  FinalizeAction = () => {
    this._previousAction.value = this._currentAction.value
    this._currentAction.value = PackingListAction.None
  }

  SetupList = (name: string) => {
    const list = this._allPackingLists.value.find((list) => list.name === name)

    if (!list) {
      this.AddList(this._selectedListName.value)
    }

    this._selectedPackingList.value = this._allPackingLists.value.find(
      (list) => list.name === name
    )!

    if (this._selectedPackingList.value.state == PackingListState.Unsynced) {
      this.FetchPackingList()
    }
  }

  AddList = (name: string) => {
    const newList = PackingList.New(name)
    this._allPackingLists.value.push(newList)
    return newList
  }

  FetchListDescriptions = async () => {
    this._allListsFetched.value = false
    const fetchedLists =
      await PackingListService.getApiPackingListListDescriptionsForUser({
        userId: this.GetUserID(),
      })
    const lists = fetchedLists.map((fetchedList) =>
      PackingList.Unsynced(fetchedList.id, fetchedList.name)
    )
    const newPackingLists = (
      this._allPackingLists.value as PackingList[]
    ).filter((list: PackingList) => list.state == PackingListState.New)
    this._allPackingLists.value.length = 0
    this._allPackingLists.value.push(...lists)
    this._allPackingLists.value.push(...newPackingLists)
    this._allListsFetched.value = true
  }

  UpdateListName = async (id: number, newName: string) => {
    const list = this._allPackingLists.value.find((list) => list.id === id)!

    if (!newName || newName === list.name) return

    list.name = newName
    if (list.state != PackingListState.New) {
      await PackingListService.postApiPackingListUpdateName({
        listId: id,
        newName: newName,
        userId: this.GetUserID(),
      })
    }
  }

  DeleteList = async (id: number) => {
    const list = this._allPackingLists.value.find((list) => list.id === id)!

    if (list.name === this._selectedListName.value) {
      this._selectedListName.value = ""
      router.push("/")
    }

    if (list.state != PackingListState.New) {
      await PackingListService.deleteApiPackingListDelete({
        listId: id,
        userId: this.GetUserID(),
      })
    }

    this._allPackingLists.value.splice(
      this._allPackingLists.value.findIndex((list) => list.id === id),
      1
    )
  }

  FetchPackingList = async () => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Fetching
    const fetchedList = await PackingListService.getApiPackingListList({
      listId: this._selectedPackingList.value.id,
    })
    this._selectedPackingList.value.Synchronize(fetchedList, fetchedList.name)
    this.FinalizeAction()
  }

  SynchronizeGroup = async (groupID: number) => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Fetching
    const fetchedList = await PackingListService.getApiPackingListList({
      listId: this._selectedPackingList.value.id,
    })
    const jsonGroup = fetchedList.content.groups.find(
      (group) => group.id === groupID
    )!
    const fetchedGroup = PackingGroup.FromJson(jsonGroup)
    this._selectedPackingList.value.content
      .GetGroup(fetchedGroup.id)!
      .Synchronize(fetchedGroup)
    this.FinalizeAction()
  }

  SavePackingList = async () => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Saving
    const apiList = this._selectedPackingList.value.ToJson()
    const listID = await PackingListService.postApiPackingListSave({
      userId: this.GetUserID(),
      requestBody: apiList,
    })
    this._selectedPackingList.value.id = listID
    this._selectedPackingList.value.content.groups.forEach((group) => {
      group.isNew = false
      group.items.forEach((item) => {
        item.isNew = false
      })
    })
    this._selectedPackingList.value.state = PackingListState.Synced
    this.FinalizeAction()
  }

  CopyPackingList = async (listWithNameToCopy: string) => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Copying
    const listToCopy = this._allPackingLists.value.find(
      (list) => list.name === listWithNameToCopy
    )!
    const copiedListName = `${listWithNameToCopy} - Copy`
    const copiedList = this.AddList(copiedListName)

    if (listToCopy.state == PackingListState.Unsynced) {
      const fetchedList = await PackingListService.getApiPackingListList({
        listId: listToCopy.id,
      })
      copiedList.Synchronize(fetchedList, copiedListName)
    } else {
      copiedList.Synchronize(listToCopy, copiedListName)
    }

    copiedList.state = PackingListState.New
    this.FinalizeAction()
    return copiedListName
  }

  GetGroup = (id: number) => {
    return this._selectedPackingList.value.content.GetGroup(id)!
  }

  AddGroup = (name: string, type: PackingItemType) => {
    return this._selectedPackingList.value.content.AddGroup(name, type)
  }

  CopyGroups = (groupIDs: number[], cut: boolean) => {
    const list = this._allPackingLists.value.find(
      (list) => list.name === this._selectedListName.value
    )! as PackingList

    this._copiedGroups.value.length = 0
    this._copiedItems.value.length = 0

    groupIDs.forEach((groupID) => {
      const groupToCopy = list.content.GetGroup(groupID)!
      const copiedGroup = PackingGroup.Undefined()
      copiedGroup.Synchronize(groupToCopy) //TODO fix item IDs
      this._copiedGroups.value.push(copiedGroup)
      if (cut) {
        list.content.RemoveGroup(groupID)
      }
    })
  }

  CopyItems = (groupID: number, itemIDs: number[], cut: boolean) => {
    const list = this._allPackingLists.value.find(
      (list) => list.name === this._selectedListName.value
    )! as PackingList

    this._copiedGroups.value.length = 0
    this._copiedItems.value.length = 0
    const group = list.content.GetGroup(groupID)!

    itemIDs.forEach((itemID) => {
      const itemToCopy = group.items.find((item) => item.id == itemID)!
      const copiedItem = PackingItem.Undefined()
      copiedItem.Synchronize(itemToCopy)
      this._copiedItems.value.push(copiedItem)
      if (cut) {
        group.RemoveItem(itemID)
      }
    })
  }

  AddItem = (
    itemName: string,
    itemCount: number,
    itemAttributes: PackingItemAttribute[],
    groupID: number
  ) => {
    const addedItem = this._selectedPackingList.value.content
      .GetGroup(groupID)!
      .AddItem(
        this._selectedPackingList.value.content.GetNextItemID(),
        itemName,
        itemCount
      )
    addedItem.attributes = [...itemAttributes]
  }

  RemoveItem = (itemID: number, groupID: number) => {
    this._selectedPackingList.value.content
      .GetGroup(groupID)!
      .RemoveItem(itemID)
  }

  RemoveGroup = (groupID: number) => {
    this._selectedPackingList.value.content.RemoveGroup(groupID)
  }

  SortItemUp = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.content.GetGroup(groupID)!
    const currentItem = group.items.find((item) => item.id == itemID)!
    const previousItem = this.GetPreviousItem(groupID, itemID)

    if (previousItem.id !== undefined) {
      const currentItemSort = currentItem.sort
      currentItem.sort = previousItem.sort
      previousItem.sort = currentItemSort
      this._selectedPackingList.value.content.GetGroup(groupID)!.SortItems()
    }
  }

  SortItemDown = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.content.GetGroup(groupID)!
    const currentItem = group.items.find((item) => item.id == itemID)!
    const nextItem = this.GetNextItem(groupID, itemID)

    if (nextItem.id !== undefined) {
      const currentItemSort = currentItem.sort
      currentItem.sort = nextItem.sort
      nextItem.sort = currentItemSort
      this._selectedPackingList.value.content.GetGroup(groupID)!.SortItems()
    }
  }

  GetPreviousItem = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.content.GetGroup(groupID)!
    const currentItem = group.items.find((item) => item.id == itemID)!
    const otherItems = group.items.filter((item) => item.id != currentItem.id)!

    let previousItem = {} as PackingItem
    otherItems.forEach((item) => {
      if (
        item.sort < currentItem.sort &&
        (previousItem.id === undefined || previousItem.sort < item.sort)
      ) {
        previousItem = item
      }
    })

    return previousItem
  }

  GetNextItem = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.content.GetGroup(groupID)!
    const currentItem = group.items.find((item) => item.id == itemID)!
    const otherItems = group.items.filter((item) => item.id != currentItem.id)!

    let nextItem = {} as PackingItem
    otherItems.forEach((item) => {
      if (
        item.sort > currentItem.sort &&
        (nextItem.id === undefined || nextItem.sort > item.sort)
      ) {
        nextItem = item
      }
    })

    return nextItem
  }
}
