import type { IGridElement } from "../grid/IGridElement"
import { PackingItem } from "../item/PackingItem"
import type { PackingItemAttribute } from "../item/PackingItemAttribute"
import { PackingItemType } from "../item/PackingItemType"
import type { IPackingGroup } from "./IPackingGroup"

export class PackingGroup implements IPackingGroup, IGridElement {
  readonly ID: number
  Name: string
  Type: PackingItemType
  Sort: number
  IsNew: boolean
  Items: PackingItem[]

  private constructor(
    id: number,
    name: string,
    type: PackingItemType,
    sort: number,
    isNew: boolean,
    items: PackingItem[]
  ) {
    this.ID = id
    this.Name = name
    this.Type = type
    this.Sort = sort
    this.IsNew = isNew
    this.Items = items
  }

  static New(id: number, name: string, type: PackingItemType, sort: number) {
    return new PackingGroup(id, name, type, sort, true, [])
  }

  static Undefined() {
    return new PackingGroup(0, "", PackingItemType.Other, 0, false, [])
  }

  static FromJson(json: IPackingGroup) {
    return new PackingGroup(
      json.ID,
      json.Name,
      json.Type,
      json.Sort,
      false,
      json.Items.map((item) => PackingItem.FromJson(item))
    )
  }

  ToJson() {
    return <IPackingGroup>{
      ID: this.ID,
      Name: this.Name,
      Type: this.Type,
      Sort: this.Sort,
      Items: this.Items.map((item) => item.ToJson()),
    }
  }

  Synchronize(otherGroup: IPackingGroup) {
    this.Name = otherGroup.Name
    this.Type = otherGroup.Type
    this.Sort = otherGroup.Sort
    this.Items.length = 0
    this.AddItems(otherGroup.Items.map((item) => PackingItem.FromJson(item)))
  }

  AddItem(id: number, name: string, count: number) {
    const sort = this.Items.length
      ? Math.max.apply(
          Math,
          this.Items.map((item) => item.Sort)
        ) + 1
      : 1

    const newItem = PackingItem.New(
      id,
      name,
      count,
      sort,
      [] as PackingItemAttribute[]
    )

    this.Items.push(newItem)
    return newItem
  }

  AddItems(newItems: PackingItem[]) {
    this.Items = this.Items.concat(newItems)
  }

  RemoveItem(itemID: number) {
    this.Items.splice(
      this.Items.findIndex((item) => item.ID === itemID),
      1
    )
  }

  SortItems() {
    this.Items.sort((a, b) => {
      return a.Sort - b.Sort
    })
  }
}
