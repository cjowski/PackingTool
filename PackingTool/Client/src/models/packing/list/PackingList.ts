import type { IPackingList } from "./IPackingList"
import type { PackingItemType } from "../item/PackingItemType"
import { PackingGroup } from "../group/PackingGroup"
import { PackingListState } from "./PackingListState"

export class PackingList implements IPackingList {
  Name: string
  GridColumnCount: number
  Groups: PackingGroup[]
  State: PackingListState

  constructor(
    name: string,
    gridColumnCount: number,
    groups: PackingGroup[],
    state: PackingListState
  ) {
    this.Name = name
    this.GridColumnCount = gridColumnCount
    this.Groups = groups
    this.State = state
  }

  static New(name: string) {
    return new PackingList(name, 0, [] as PackingGroup[], PackingListState.New)
  }

  static Unsynced(name: string) {
    return new PackingList(
      name,
      0,
      [] as PackingGroup[],
      PackingListState.Unsynced
    )
  }

  static Undefined() {
    return new PackingList(
      "",
      0,
      [] as PackingGroup[],
      PackingListState.Undefined
    )
  }

  ToJson() {
    return <IPackingList>{
      GridColumnCount: this.GridColumnCount,
      Groups: this.Groups.map((group) => group.ToJson()),
    }
  }

  GetGroup(id: number) {
    return this.Groups.find((group) => group.ID === id)
  }

  Synchronize(otherList: IPackingList, otherListName: string) {
    this.Name = otherListName
    this.GridColumnCount = otherList.GridColumnCount

    otherList.Groups.forEach((otherGroup) => {
      const group = this.GetGroup(otherGroup.ID)
      if (!group) {
        this.Groups.push(PackingGroup.FromJson(otherGroup))
      } else {
        group.Synchronize(otherGroup)
      }
    })

    const groupIDsToRemove = [] as number[]
    this.Groups.forEach((group) => {
      if (!otherList.Groups.find((otherGroup) => otherGroup.ID === group.ID)) {
        groupIDsToRemove.push(group.ID)
      }
    })

    groupIDsToRemove.forEach((groupID) => {
      this.Groups.splice(
        this.Groups.findIndex((group) => group.ID === groupID),
        1
      )
    })

    this.SortGroups()
    this.State = PackingListState.Synced
  }

  AddGroup(name: string, type: PackingItemType) {
    const sort = this.Groups.length
      ? Math.max.apply(
          Math,
          this.Groups.map((group) => group.Sort)
        ) + 1
      : 1

    const group = PackingGroup.New(this.GetNextGroupID(), name, type, sort)
    this.Groups.push(group)

    if (this.GridColumnCount < 3) {
      ++this.GridColumnCount
    }

    return group
  }

  RemoveGroup(id: number) {
    this.Groups.splice(
      this.Groups.findIndex((group) => group.ID === id),
      1
    )
  }

  SortGroups() {
    this.Groups.sort((a, b) => {
      return a.Sort - b.Sort
    })
  }

  GetNextGroupID() {
    return this.Groups.length
      ? Math.max.apply(
          Math,
          this.Groups.map((group) => group.ID)
        ) + 1
      : 1
  }

  GetNextItemID() {
    return (
      Math.max.apply(
        Math,
        this.Groups.map((group) =>
          Math.max.apply(
            Math,
            group.Items.length ? group.Items.map((item) => item.ID) : [0]
          )
        )
      ) + 1
    )
  }
}
