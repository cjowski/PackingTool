import type { IPackingItem } from "./IPackingItem"
import type { PackingItemAttribute } from "./PackingItemAttribute"

export class PackingItem implements IPackingItem {
  readonly ID: number
  Name: string
  Count: number
  Sort: number
  Attributes: PackingItemAttribute[]
  IsNew: boolean
  Packed: boolean

  private constructor(
    id: number,
    name: string,
    count: number,
    sort: number,
    attributes: PackingItemAttribute[],
    isNew: boolean,
    packed: boolean
  ) {
    this.ID = id
    this.Name = name
    this.Count = count
    this.Sort = sort
    this.Attributes = attributes
    this.IsNew = isNew
    this.Packed = packed
  }

  static New(
    id: number,
    name: string,
    count: number,
    sort: number,
    attributes: PackingItemAttribute[]
  ) {
    return new PackingItem(id, name, count, sort, attributes, true, false)
  }

  static Undefined() {
    return new PackingItem(
      0,
      "",
      1,
      1,
      [] as PackingItemAttribute[],
      true,
      false
    )
  }

  static FromJson(json: IPackingItem) {
    return new PackingItem(
      json.ID,
      json.Name,
      json.Count,
      json.Sort,
      json.Attributes ?? ([] as PackingItemAttribute[]),
      false,
      json.Packed ?? false
    )
  }

  ToJson() {
    return <IPackingItem>{
      ID: this.ID,
      Name: this.Name,
      Count: this.Count,
      Sort: this.Sort,
      Attributes: this.Attributes,
      Packed: this.Packed,
    }
  }

  Synchronize(otherItem: IPackingItem) {
    this.Name = otherItem.Name
    this.Count = otherItem.Count
    this.Sort = otherItem.Sort
    this.Attributes = [...otherItem.Attributes]
    this.Packed = otherItem.Packed
  }
}
