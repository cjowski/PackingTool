<template>
  <q-page padding class="flex flex-top q-mt-md">
    <div class="column" v-if="selectedListName">
      <div class="row">
        <PackingGrid :packing="packing" />
      </div>
      <div class="row" style="margin-top: 70px">
        <ImportantItems :packing="packing" />
      </div>
    </div>

    <q-dialog v-model="addingGroup">
      <AddPackingGroup :closeAddGroupDialog="() => (addingGroup = false)" />
    </q-dialog>

    <q-page-sticky position="top-right" :offset="[40, 40]">
      <q-btn
        v-if="packing"
        fab
        :icon="allGroupsPacked ? 'remove_done' : 'done_all'"
        :color="allGroupsPacked ? 'orange' : 'primary'"
        class="q-mr-md"
        @click="packAllGroups(!allGroupsPacked)"
      />
      <q-btn
        v-if="!packing"
        fab
        icon="luggage"
        color="green"
        @click="packing = true"
      />
      <q-btn
        v-else
        fab
        icon="no_luggage"
        color="deep-orange"
        @click="packing = false"
      />
    </q-page-sticky>

    <q-page-sticky position="bottom-right" :offset="[40, 115]">
      <div class="q-pa-md q-gutter-md">
        <q-btn
          v-if="!configuringGrid"
          fab
          icon="grid_view"
          color="purple"
          @click="configuringGrid = true"
        />
        <q-card v-if="configuringGrid" class="bg-purple">
          <q-card-section horizontal>
            <q-btn
              icon="check"
              color="purple"
              @click="configuringGrid = false"
            />
            <q-input
              v-model="packingList.GridColumnCount"
              type="number"
              :min="minColumnsInRow"
              :max="maxColumnsInRow"
              borderless
              style="max-width: 40px"
              class="q-ml-md q-mr-sm"
            ></q-input>
          </q-card-section>
        </q-card>
      </div>
    </q-page-sticky>

    <q-page-sticky position="bottom-right" :offset="[40, 40]">
      <div class="q-pa-md q-gutter-md">
        <q-btn
          fab
          icon="playlist_add"
          color="cyan"
          @click="addingGroup = true"
        />
        <q-btn
          v-if="packingList.State != PackingListState.New"
          fab
          icon="sync"
          color="orange"
          @click="packingListManager.FetchPackingList"
        />
        <q-btn
          fab
          icon="save"
          color="blue"
          @click="packingListManager.SavePackingList"
        />
      </div>
    </q-page-sticky>
  </q-page>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { useRoute } from "vue-router"
import { useQuasar, QSpinnerGrid } from "quasar"
import { storeToRefs } from "pinia"
import { usePackingListStore } from "@/stores/packingListStore"
import PackingGrid from "@/components/packing/list/PackingGrid.vue"
import ImportantItems from "@/components/packing/list/group/ImportantItems.vue"
import AddPackingGroup from "@/components/packing/list/AddPackingGroup.vue"
import { PackingListState } from "@/models/packing/list/PackingListState"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { PackingListAction } from "@/enums/PackingListAction"

const { packingListManager } = usePackingListStore()
const { packingList, selectedListName, allListsFetched } = storeToRefs(
  usePackingListStore()
)
const { currentAction, previousAction } = storeToRefs(useOperationStatusStore())

const route = useRoute()
const $q = useQuasar()
const packing = ref(false)
const addingGroup = ref(false)
const configuringGrid = ref(false)
selectedListName.value = route.query.name?.toString() ?? ""

const minColumnsInRow = computed(() => {
  return packingList.value.Groups.length < 2
    ? packingList.value.Groups.length
    : 2
})

const maxColumnsInRow = computed(() => {
  return packingList.value.Groups.length > 7
    ? 7
    : packingList.value.Groups.length
})

const allGroupsPacked = computed({
  get() {
    return packingList.value.Groups.every((group) =>
      group.Items.every((item) => item.Packed)
    )
  },
  set(value: boolean) {
    packAllGroups(value)
  },
})

const packAllGroups = (value: boolean) => {
  packingList.value.Groups.forEach((group) =>
    group.Items.forEach((item) => (item.Packed = value))
  )
}

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
