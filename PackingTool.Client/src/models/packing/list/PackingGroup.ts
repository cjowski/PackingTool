import type { PackingGroup as ApiPackingGroup } from "src/api/models/PackingGroup"
import type { PackingItemType } from "src/api/models/PackingItemType"
import type { PackingItemAttribute } from "src/api/models/PackingItemAttribute"
import type { IGridElement } from "../grid/IGridElement"
import { PackingItem } from "./PackingItem"
import { ExistenceStatus } from "./ExistenceStatus"

export class PackingGroup implements ApiPackingGroup, IGridElement {
  readonly id: number
  name: string
  type: PackingItemType
  sort: number
  status: ExistenceStatus
  items: PackingItem[]

  private constructor(
    id: number,
    name: string,
    type: PackingItemType,
    sort: number,
    status: ExistenceStatus,
    items: PackingItem[]
  ) {
    this.id = id
    this.name = name
    this.type = type
    this.sort = sort
    this.status = status
    this.items = items
  }

  static New(id: number, name: string, type: PackingItemType, sort: number) {
    return new PackingGroup(id, name, type, sort, ExistenceStatus.New, [])
  }

  static Undefined() {
    return new PackingGroup(0, "", "Other", 0, ExistenceStatus.Default, [])
  }

  static FromJson(json: ApiPackingGroup) {
    return new PackingGroup(
      json.id,
      json.name,
      json.type,
      json.sort,
      ExistenceStatus.Default,
      json.items.map((item) => PackingItem.FromJson(item))
    )
  }

  ToJson() {
    return <ApiPackingGroup>{
      id: this.id,
      name: this.name,
      type: this.type,
      sort: this.sort,
      items: this.items.map((item) => item.ToJson()),
    }
  }

  Synchronize(otherGroup: ApiPackingGroup) {
    this.name = otherGroup.name
    this.type = otherGroup.type
    this.sort = otherGroup.sort
    this.items.length = 0
    this.AddItems(otherGroup.items.map((item) => PackingItem.FromJson(item)))
  }

  AddItem(id: number, name: string, count: number) {
    const sort = this.items.length
      ? Math.max.apply(
          Math,
          this.items.map((item) => item.sort)
        ) + 1
      : 1

    const newItem = PackingItem.New(
      id,
      name,
      count,
      sort,
      [] as PackingItemAttribute[]
    )

    this.items.push(newItem)
    return newItem
  }

  AddItems(newItems: PackingItem[]) {
    this.items = this.items.concat(newItems)
  }

  RemoveItem(itemID: number) {
    this.items.splice(
      this.items.findIndex((item) => item.id === itemID),
      1
    )
  }

  SortItems() {
    this.items.sort((a, b) => {
      return a.sort - b.sort
    })
  }
}
