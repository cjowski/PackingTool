import axios from "axios"
import {
  getStorage,
  ref,
  listAll,
  getDownloadURL,
  uploadBytes,
  deleteObject,
} from "firebase/storage"

export class FirebaseStorage {
  private static readonly FIREBASE_ORIGIN =
    "https://firebasestorage.googleapis.com" //get somehow this firebase url

  static GetFileNames = async (folderName: string, extension: string) => {
    const storage = getStorage()
    const filesReference = ref(storage, folderName)
    const fileNames = [] as string[]

    await listAll(filesReference)
      .then((res) => {
        res.items.forEach((itemRef) => {
          if (itemRef.name.endsWith(extension)) {
            fileNames.push(itemRef.name)
          }
        })
      })
      .catch((error) => {
        throw error
      })

    return fileNames
  }

  static GetJson = async (jsonPath: string) => {
    const storage = getStorage()
    const fileReference = ref(storage, jsonPath)
    const originalUrl = await getDownloadURL(fileReference)
    const downloadUrl = originalUrl.replace(
      this.FIREBASE_ORIGIN,
      window.location.origin + "/storage"
    )
    const response = await axios.get(downloadUrl)
    return response.data
  }

  static SaveJson = async (json: string, jsonPath: string) => {
    const storage = getStorage()
    const fileReference = ref(storage, jsonPath)
    const fileBlob = new Blob([json], { type: "application/json" })
    uploadBytes(fileReference, fileBlob).then(() => {
      console.log(`Saved json: ${jsonPath}!`)
    })
  }

  static DeleteJson = async (jsonPath: string) => {
    const storage = getStorage()
    const fileReference = ref(storage, jsonPath)
    deleteObject(fileReference).then(() => {
      console.log(`Removed json: ${jsonPath}!`)
    })
  }
}
