import { FirebaseStorage } from "../FirebaseStorage"
import type { IPackingList } from "@/models/packing/list/IPackingList"

export class PackingListStorage {
  private static readonly PACKING_LIST_PATH = "packingList"
  private static readonly REMOVED_PACKING_LIST_PATH = `${PackingListStorage.PACKING_LIST_PATH}/Removed`

  static GetList = async (name: string) => {
    return await FirebaseStorage.GetJson(
      `${PackingListStorage.PACKING_LIST_PATH}/${name}.json`
    )
  }

  static GetListNames = async () => {
    const fileNames = await FirebaseStorage.GetFileNames(
      PackingListStorage.PACKING_LIST_PATH,
      ".json"
    )
    return fileNames.map((fileName) => {
      return fileName.replace(".json", "")
    })
  }

  static UpdateListName = async (currentName: string, newName: string) => {
    const currentJsonPath = `${PackingListStorage.PACKING_LIST_PATH}/${currentName}.json`
    const newJsonPath = `${PackingListStorage.PACKING_LIST_PATH}/${newName}.json`
    const json = await FirebaseStorage.GetJson(currentJsonPath)
    await FirebaseStorage.SaveJson(JSON.stringify(json), newJsonPath)
    await FirebaseStorage.DeleteJson(currentJsonPath)
  }

  static SaveList = async (list: IPackingList, listName: string) => {
    await FirebaseStorage.SaveJson(
      JSON.stringify(list),
      `${PackingListStorage.PACKING_LIST_PATH}/${listName}.json`
    )
  }

  static DeleteList = async (name: string) => {
    const jsonPath = `${PackingListStorage.PACKING_LIST_PATH}/${name}.json`
    const recycleJsonPath = `${PackingListStorage.REMOVED_PACKING_LIST_PATH}/${name}.json`
    const json = await FirebaseStorage.GetJson(jsonPath)
    await FirebaseStorage.SaveJson(JSON.stringify(json), recycleJsonPath)
    await FirebaseStorage.DeleteJson(jsonPath)
  }
}
