<template>
  <q-drawer
    v-model="packingListToolbarShown"
    side="right"
    behavior="desktop"
    bordered
    overlay
    persistent
    :width="packingListsShown ? 380 : 80"
  >
    <q-scroll-area class="fit">
      <q-btn
        v-if="packing"
        fab
        :icon="allGroupsPacked ? 'remove_done' : 'done_all'"
        :color="allGroupsPacked ? 'orange' : 'primary'"
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
      <q-space />
      <q-btn
        v-if="!configuringGrid"
        fab
        icon="grid_view"
        color="purple"
        @click="configuringGrid = true"
      />
      <q-card v-if="configuringGrid" class="bg-purple">
        <q-card-section horizontal>
          <q-btn icon="check" color="purple" @click="configuringGrid = false" />
          <q-input
            v-model="packingList.content.gridColumnCount"
            type="number"
            :min="minColumnsInRow"
            :max="maxColumnsInRow"
            borderless
            style="max-width: 40px"
            class="q-ml-md q-mr-sm"
          ></q-input>
        </q-card-section>
      </q-card>
      <q-btn fab icon="playlist_add" color="cyan" @click="addingGroup = true" />
      <q-btn
        v-if="packingList.state != PackingListState.New"
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

      <q-dialog v-model="addingGroup">
        <AddPackingGroup :closeAddGroupDialog="() => (addingGroup = false)" />
      </q-dialog>
    </q-scroll-area>
  </q-drawer>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import { storeToRefs } from "pinia"
import { useRoute } from "vue-router"
import AddPackingGroup from "@/components/packing/list/AddPackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { PackingListState } from "@/models/packing/list/PackingListState"

const { packingListManager } = useAllPackingListsStore()
const { selectedListName, packingListsShown } = storeToRefs(
  useAllPackingListsStore()
)
const { packingList, packing } = storeToRefs(useOpenedPackingListStore())

const route = useRoute()
const addingGroup = ref(false)
const configuringGrid = ref(false)
selectedListName.value = route.query.name?.toString() ?? ""

const packingListToolbarShown = computed(() => !!selectedListName)

const minColumnsInRow = computed(() => {
  return packingList.value.content.groups.length < 2
    ? packingList.value.content.groups.length
    : 2
})

const maxColumnsInRow = computed(() => {
  return packingList.value.content.groups.length > 7
    ? 7
    : packingList.value.content.groups.length
})

const allGroupsPacked = computed({
  get() {
    return packingList.value.content.groups.every((group) =>
      group.items.every((item) => item.packed)
    )
  },
  set(value: boolean) {
    packAllGroups(value)
  },
})

const packAllGroups = (value: boolean) => {
  packingList.value.content.groups.forEach((group) =>
    group.items.forEach((item) => (item.packed = value))
  )
}
</script>
