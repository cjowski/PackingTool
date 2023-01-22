import { ref } from "vue"
import { defineStore } from "pinia"
import { PackingListAction } from "@/enums/PackingListAction"
import { PackingSectionType } from "@/enums/PackingSectionType"

export const useOperationStatusStore = defineStore("operationStatus", () => {
  const currentAction = ref(PackingListAction.None)
  const previousAction = ref(PackingListAction.None)
  const currentSectionFocus = ref(PackingSectionType.None)
  const currentGroupIDFocus = ref(0)

  return {
    currentAction,
    previousAction,
    currentSectionFocus,
    currentGroupIDFocus,
  }
})
