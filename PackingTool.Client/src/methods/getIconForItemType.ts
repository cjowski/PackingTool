import type { PackingItemType } from "src/api/models/PackingItemType"

export default function getIconByItemType(type: PackingItemType) {
  switch (type) {
    case "Cloth":
      return "checkroom"
    case "Electronics":
      return "phone_android"
    case "Food":
      return "restaurant"
    case "Documents":
      return "article"
    case "Shoes":
      return "pets"
    case "Cosmetics":
      return "science"
    case "Medicaments":
      return "local_hospital"
    case "Other":
      return "auto_awesome"
    default:
      return ""
  }
}
