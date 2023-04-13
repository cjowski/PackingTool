<template>
  <q-page padding class="flex flex-top q-mt-md">
    <div class="column" v-if="selectedListName">
      <div v-if="$q.screen.gt.sm" class="row">
        <PackingGrid />
      </div>
      <div
        v-else
        v-for="group in packingList.content.groups"
        class="row q-mt-md"
      >
        <PackingGroup :group="packingListManager.GetGroup(group.id)" />
      </div>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted, watch } from "vue"
import { useRoute } from "vue-router"
import { useQuasar, QSpinnerGrid } from "quasar"
import { storeToRefs } from "pinia"
import PackingGrid from "@/components/packing/list/PackingGrid.vue"
import PackingGroup from "@/components/packing/list/group/PackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { PackingListAction } from "@/enums/PackingListAction"

const { packingListManager } = useAllPackingListsStore()
const { selectedListName, allListsFetched } = storeToRefs(
  useAllPackingListsStore()
)
const { packingList, packing } = storeToRefs(useOpenedPackingListStore())
const { currentAction, previousAction } = storeToRefs(useOperationStatusStore())

const route = useRoute()
const $q = useQuasar()
selectedListName.value = route.query.name?.toString() ?? ""

watch(
  route,
  () => {
    if (allListsFetched.value) {
      selectedListName.value = route.query.name?.toString() ?? ""
      packing.value = false
    }
  },
  { flush: "pre", immediate: true, deep: true }
)

watch([currentAction, previousAction], ([action, prevAction]) => {
  if (
    action == PackingListAction.Fetching ||
    action == PackingListAction.Copying
  ) {
    $q.loading.show({
      spinner: QSpinnerGrid,
      spinnerColor: "orange-6",
      spinnerSize: 140,
      message: "Fetching...",
      messageColor: "orange-6",
    })
  } else if (
    action == PackingListAction.None &&
    (prevAction == PackingListAction.Fetching ||
      prevAction == PackingListAction.Copying)
  ) {
    $q.loading.hide()
  }
})

watch([currentAction, previousAction], ([action, prevAction]) => {
  if (action == PackingListAction.Saving) {
    $q.loading.show({
      spinner: QSpinnerGrid,
      spinnerColor: "cyan",
      spinnerSize: 140,
      message: "Saving...",
      messageColor: "cyan",
    })
  } else if (
    action == PackingListAction.None &&
    prevAction == PackingListAction.Saving
  ) {
    $q.loading.hide()
    $q.notify({
      message: "Saved",
      color: "blue",
      icon: "done",
    })
  }
})

const savePackingList = (event: KeyboardEvent) => {
  if (event.ctrlKey && event.key.toLowerCase() === "s") {
    event.preventDefault()
    packingListManager.SavePackingList()
  }
}

onMounted(() => {
  window.addEventListener("keydown", savePackingList)
})

onUnmounted(() => {
  window.removeEventListener("keydown", savePackingList)
})
</script>
