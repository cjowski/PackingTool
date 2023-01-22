import type { Ref } from "vue"
import router from "@/router"
import type { IPackingList } from "@/models/packing/list/IPackingList"
import type { PackingItemType } from "@/models/packing/item/PackingItemType"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingListStorage } from "./PackingListStorage"
import { PackingGroup } from "@/models/packing/group/PackingGroup"
import { PackingListState } from "@/models/packing/list/PackingListState"
import { PackingList } from "@/models/packing/list/PackingList"
import { PackingItem } from "@/models/packing/item/PackingItem"
import type { PackingItemAttribute } from "@/models/packing/item/PackingItemAttribute"

export class PackingListManager {
  private _allPackingLists: Ref<PackingList[]>
  private _selectedPackingList: Ref<PackingList>
  private _selectedListName: Ref<string>
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

  AnyActionInProgress = () => {
    return this._currentAction.value != PackingListAction.None
  }

  FinalizeAction = () => {
    this._previousAction.value = this._currentAction.value
    this._currentAction.value = PackingListAction.None
  }

  SetupList = (name: string) => {
    const list = this._allPackingLists.value.find((list) => list.Name === name)

    if (!list) {
      this.AddList(this._selectedListName.value)
    }

    this._selectedPackingList.value = this._allPackingLists.value.find(
      (list) => list.Name === name
    )!

    if (this._selectedPackingList.value.State == PackingListState.Unsynced) {
      this.FetchPackingList()
    }
  }

  AddList = (name: string) => {
    const newList = PackingList.New(name)
    this._allPackingLists.value.push(newList)
    return newList
  }

  GetFetchedListJson = async (listName: string) => {
    return PackingListStorage.GetList(listName)
  }

  FetchListNames = async () => {
    this._allListsFetched.value = false
    const listNames = await PackingListStorage.GetListNames()
    const fetchedLists = listNames.map((listName: string) =>
      PackingList.Unsynced(listName)
    )
    const newPackingLists = (
      this._allPackingLists.value as PackingList[]
    ).filter((list: PackingList) => list.State == PackingListState.New)
    this._allPackingLists.value.length = 0
    this._allPackingLists.value.push(...fetchedLists)
    this._allPackingLists.value.push(...newPackingLists)
    this._allListsFetched.value = true
  }

  UpdateListName = async (currentName: string, newName: string) => {
    if (!newName || newName === currentName) return

    const list = this._allPackingLists.value.find(
      (list) => list.Name === currentName
    )!
    list.Name = newName

    if (list.State != PackingListState.New) {
      PackingListStorage.UpdateListName(currentName, newName)
    }
  }

  DeleteList = async (name: string) => {
    const list = this._allPackingLists.value.find((list) => list.Name === name)!

    if (name === this._selectedListName.value) {
      this._selectedListName.value = ""
      router.push("/")
    }

    if (list.State != PackingListState.New) {
      PackingListStorage.DeleteList(name)
    }

    this._allPackingLists.value.splice(
      this._allPackingLists.value.findIndex((list) => list.Name === name),
      1
    )
  }

  FetchPackingList = async () => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Fetching
    const jsonList = (await PackingListStorage.GetList(
      this._selectedListName.value
    )) as IPackingList
    this._selectedPackingList.value.Synchronize(
      jsonList,
      this._selectedListName.value
    )
    this.FinalizeAction()
  }

  SynchronizeGroup = async (groupID: number) => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Fetching
    const jsonList = (await PackingListStorage.GetList(
      this._selectedListName.value
    )) as IPackingList
    const jsonGroup = jsonList.Groups.find((group) => group.ID === groupID)!
    const fetchedGroup = PackingGroup.FromJson(jsonGroup)
    this._selectedPackingList.value
      .GetGroup(fetchedGroup.ID)!
      .Synchronize(fetchedGroup)
    this.FinalizeAction()
  }

  SavePackingList = async () => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Saving
    const jsonList = this._selectedPackingList.value.ToJson()
    await PackingListStorage.SaveList(jsonList, this._selectedListName.value)
    this._selectedPackingList.value.Groups.forEach((group) => {
      group.IsNew = false
      group.Items.forEach((item) => {
        item.IsNew = false
      })
    })
    this._selectedPackingList.value.State = PackingListState.Synced
    this.FinalizeAction()
  }

  CopyPackingList = async (listWithNameToCopy: string) => {
    if (this.AnyActionInProgress()) return
    this._currentAction.value = PackingListAction.Copying
    const listToCopy = this._allPackingLists.value.find(
      (list) => list.Name === listWithNameToCopy
    )!
    const copiedListName = `${listWithNameToCopy} - Copy`
    const copiedList = this.AddList(copiedListName)

    if (listToCopy.State == PackingListState.Unsynced) {
      const jsonList = (await PackingListStorage.GetList(
        listWithNameToCopy
      )) as IPackingList
      copiedList.Synchronize(jsonList, copiedListName)
    } else {
      copiedList.Synchronize(listToCopy, copiedListName)
    }

    copiedList.State = PackingListState.New
    this.FinalizeAction()
    return copiedListName
  }

  GetGroup = (id: number) => {
    return this._selectedPackingList.value.GetGroup(id)!
  }

  AddGroup = (name: string, type: PackingItemType) => {
    return this._selectedPackingList.value.AddGroup(name, type)
  }

  CopyGroups = (groupIDs: number[], cut: boolean) => {
    const list = this._allPackingLists.value.find(
      (list) => list.Name === this._selectedListName.value
    )! as PackingList

    this._copiedGroups.value.length = 0
    this._copiedItems.value.length = 0

    groupIDs.forEach((groupID) => {
      const groupToCopy = list.GetGroup(groupID)!
      const copiedGroup = PackingGroup.Undefined()
      copiedGroup.Synchronize(groupToCopy) //TODO fix item IDs
      this._copiedGroups.value.push(copiedGroup)
      if (cut) {
        list.RemoveGroup(groupID)
      }
    })
  }

  CopyItems = (groupID: number, itemIDs: number[], cut: boolean) => {
    const list = this._allPackingLists.value.find(
      (list) => list.Name === this._selectedListName.value
    )! as PackingList

    this._copiedGroups.value.length = 0
    this._copiedItems.value.length = 0
    const group = list.GetGroup(groupID)!

    itemIDs.forEach((itemID) => {
      const itemToCopy = group.Items.find((item) => item.ID == itemID)!
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
    const addedItem = this._selectedPackingList.value
      .GetGroup(groupID)!
      .AddItem(
        this._selectedPackingList.value.GetNextItemID(),
        itemName,
        itemCount
      )
    addedItem.Attributes = [...itemAttributes]
  }

  RemoveItem = (itemID: number, groupID: number) => {
    this._selectedPackingList.value.GetGroup(groupID)!.RemoveItem(itemID)
  }

  RemoveGroup = (groupID: number) => {
    this._selectedPackingList.value.RemoveGroup(groupID)
  }

  SortItemUp = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.GetGroup(groupID)!
    const currentItem = group.Items.find((item) => item.ID == itemID)!
    const previousItem = this.GetPreviousItem(groupID, itemID)

    if (previousItem.ID !== undefined) {
      const currentItemSort = currentItem.Sort
      currentItem.Sort = previousItem.Sort
      previousItem.Sort = currentItemSort
      this._selectedPackingList.value.GetGroup(groupID)!.SortItems()
    }
  }

  SortItemDown = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.GetGroup(groupID)!
    const currentItem = group.Items.find((item) => item.ID == itemID)!
    const nextItem = this.GetNextItem(groupID, itemID)

    if (nextItem.ID !== undefined) {
      const currentItemSort = currentItem.Sort
      currentItem.Sort = nextItem.Sort
      nextItem.Sort = currentItemSort
      this._selectedPackingList.value.GetGroup(groupID)!.SortItems()
    }
  }

  GetPreviousItem = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.GetGroup(groupID)!
    const currentItem = group.Items.find((item) => item.ID == itemID)!
    const otherItems = group.Items.filter((item) => item.ID != currentItem.ID)!

    let previousItem = {} as PackingItem
    otherItems.forEach((item) => {
      if (
        item.Sort < currentItem.Sort &&
        (previousItem.ID === undefined || previousItem.Sort < item.Sort)
      ) {
        previousItem = item
      }
    })

    return previousItem
  }

  GetNextItem = (groupID: number, itemID: number) => {
    const group = this._selectedPackingList.value.GetGroup(groupID)!
    const currentItem = group.Items.find((item) => item.ID == itemID)!
    const otherItems = group.Items.filter((item) => item.ID != currentItem.ID)!

    let nextItem = {} as PackingItem
    otherItems.forEach((item) => {
      if (
        item.Sort > currentItem.Sort &&
        (nextItem.ID === undefined || nextItem.Sort > item.Sort)
      ) {
        nextItem = item
      }
    })

    return nextItem
  }
}
