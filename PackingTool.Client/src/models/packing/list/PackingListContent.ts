import type { PackingListContent as ApiPackingListContent } from "src/api/models/PackingListContent"
import type { PackingItemType } from "src/api/models/PackingItemType"
import { PackingGroup } from "./PackingGroup"

export class PackingListContent implements ApiPackingListContent {
  gridColumnCount: number
  groups: PackingGroup[]

  constructor(gridColumnCount: number, groups: PackingGroup[]) {
    this.gridColumnCount = gridColumnCount
    this.groups = groups
  }

  static New() {
    return new PackingListContent(0, [] as PackingGroup[])
  }

  ToJson() {
    return <ApiPackingListContent>{
      gridColumnCount: this.gridColumnCount,
      groups: this.groups.map((group) => group.ToJson()),
    }
  }

  GetGroup(id: number) {
    return this.groups.find((group) => group.id === id)
  }

  Synchronize(otherListContent: ApiPackingListContent) {
    this.gridColumnCount = otherListContent.gridColumnCount

    otherListContent.groups.forEach((otherGroup) => {
      const group = this.GetGroup(otherGroup.id)
      if (!group) {
        this.groups.push(PackingGroup.FromJson(otherGroup))
      } else {
        group.Synchronize(otherGroup)
      }
    })

    const groupIDsToRemove = [] as number[]
    this.groups.forEach((group) => {
      if (
        !otherListContent.groups.find(
          (otherGroup) => otherGroup.id === group.id
        )
      ) {
        groupIDsToRemove.push(group.id)
      }
    })

    groupIDsToRemove.forEach((groupID) => {
      this.groups.splice(
        this.groups.findIndex((group) => group.id === groupID),
        1
      )
    })

    this.SortGroups()
  }

  AddGroup(name: string, type: PackingItemType) {
    const sort = this.groups.length
      ? Math.max.apply(
          Math,
          this.groups.map((group) => group.sort)
        ) + 1
      : 1

    const group = PackingGroup.New(this.GetNextGroupID(), name, type, sort)
    this.groups.push(group)

    if (this.gridColumnCount < 3) {
      ++this.gridColumnCount
    }

    return group
  }

  RemoveGroup(id: number) {
    this.groups.splice(
      this.groups.findIndex((group) => group.id === id),
      1
    )
  }

  SortGroups() {
    this.groups.sort((a, b) => {
      return a.sort - b.sort
    })
  }

  GetNextGroupID() {
    return this.groups.length
      ? Math.max.apply(
          Math,
          this.groups.map((group) => group.id)
        ) + 1
      : 1
  }

  GetNextItemID() {
    return (
      Math.max.apply(
        Math,
        this.groups.map((group) =>
          Math.max.apply(
            Math,
            group.items.length ? group.items.map((item) => item.id) : [0]
          )
        )
      ) + 1
    )
  }
}
