import type { PackingItem as ApiPackingItem } from "src/api/models/PackingItem"
import type { PackingItemAttribute } from "src/api/models/PackingItemAttribute"

export class PackingItem implements ApiPackingItem {
  readonly id: number
  name: string
  count: number
  sort: number
  attributes: PackingItemAttribute[]
  isNew: boolean
  packed: boolean

  private constructor(
    id: number,
    name: string,
    count: number,
    sort: number,
    attributes: PackingItemAttribute[],
    isNew: boolean,
    packed: boolean
  ) {
    this.id = id
    this.name = name
    this.count = count
    this.sort = sort
    this.attributes = attributes
    this.isNew = isNew
    this.packed = packed
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

  static FromJson(json: ApiPackingItem) {
    return new PackingItem(
      json.id,
      json.name,
      json.count,
      json.sort,
      json.attributes,
      false,
      json.packed
    )
  }

  ToJson() {
    return <ApiPackingItem>{
      id: this.id,
      name: this.name,
      count: this.count,
      sort: this.sort,
      attributes: this.attributes,
      packed: this.packed,
    }
  }

  Synchronize(otherItem: ApiPackingItem) {
    this.name = otherItem.name
    this.count = otherItem.count
    this.sort = otherItem.sort
    this.attributes = [...otherItem.attributes]
    this.packed = otherItem.packed
  }
}
