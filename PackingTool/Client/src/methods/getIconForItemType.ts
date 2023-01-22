import * as Enum from "@/models/packing/item/PackingItemType"

export default function getIconByItemType(type: Enum.PackingItemType) {
  switch (type) {
    case Enum.PackingItemType.Cloth:
      return "checkroom"
    case Enum.PackingItemType.Electronics:
      return "phone_android"
    case Enum.PackingItemType.Food:
      return "restaurant"
    case Enum.PackingItemType.Documents:
      return "article"
      case Enum.PackingItemType.Shoes:
        return "pets"
      case Enum.PackingItemType.Cosmetics:
        return "science"
      case Enum.PackingItemType.Medicaments:
        return "local_hospital"
    case Enum.PackingItemType.Other:
      return "auto_awesome"
    default:
      return ""
  }
}
