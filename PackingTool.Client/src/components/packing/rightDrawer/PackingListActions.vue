<template>
  <q-drawer
    v-model="packingListToolbarShown"
    side="right"
    behavior="desktop"
    bordered
    overlay
    persistent
    :width="rightDrawerShown ? 382 : 82"
    @click="rightDrawerShown = !rightDrawerShown"
  >
    <q-scroll-area class="fit">
      <q-list class="q-mt-md">
        <q-item class="q-pa-sm">
          <q-btn
            v-if="!packing"
            flat
            icon="luggage"
            color="green-14"
            size="lg"
            @click="packing = true"
            @click.stop=""
          />
          <q-btn
            v-else
            flat
            icon="no_luggage"
            color="deep-orange"
            size="lg"
            @click="packing = false"
            @click.stop=""
          />
        </q-item>
        <q-item class="q-pa-sm">
          <q-btn
            flat
            :icon="allGroupsPacked ? 'remove_done' : 'done_all'"
            :color="allGroupsPacked ? 'orange' : 'teal-13'"
            size="lg"
            :style="packing ? '' : 'visibility: hidden;'"
            @click="packAllGroups(!allGroupsPacked)"
            @click.stop=""
          />
        </q-item>
        <q-item class="q-pa-sm">
          <q-btn
            v-if="!configuringGrid"
            flat
            icon="grid_view"
            color="purple-13"
            size="lg"
            @click="configuringGrid = true"
            @click.stop=""
          />
          <q-input
            v-else
            type="number"
            outlined
            autofocus
            dense
            v-model="packingList.content.gridColumnCount"
            :min="minColumnsInRow"
            :max="maxColumnsInRow"
            @keydown.enter.prevent="configuringGrid = false"
            @keydown.esc.prevent="configuringGrid = false"
            @focusout="configuringGrid = false"
            @click.stop=""
            color="purple-13"
            class="q-mt-xs q-mb-sm q-ml-none q-mr-none"
          ></q-input>
        </q-item>
        <q-item class="q-pa-sm">
          <q-btn
            flat
            icon="playlist_add"
            color="cyan-13"
            size="lg"
            @click="addingGroup = true"
            @click.stop=""
          />
        </q-item>
        <q-item class="q-pa-sm">
          <q-btn
            flat
            icon="save"
            color="light-blue-13"
            size="lg"
            @click="packingListManager.SavePackingList"
            @click.stop=""
          />
        </q-item>
        <q-item class="q-pa-sm">
          <q-btn
            v-if="packingList.state != PackingListState.New"
            flat
            icon="sync"
            color="orange"
            size="lg"
            @click="packingListManager.FetchPackingList"
            @click.stop=""
        /></q-item>
      </q-list>
    </q-scroll-area>

    <q-dialog v-model="addingGroup">
      <AddPackingGroup :closeAddGroupDialog="() => (addingGroup = false)" />
    </q-dialog>
  </q-drawer>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import { storeToRefs } from "pinia"
import AddPackingGroup from "@/components/packing/list/AddPackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { PackingListState } from "@/models/packing/list/PackingListState"

const { packingListManager } = useAllPackingListsStore()
const { selectedListName, packingListsShown, rightDrawerShown } = storeToRefs(
  useAllPackingListsStore()
)
const { packingList, packing } = storeToRefs(useOpenedPackingListStore())

const addingGroup = ref(false)
const configuringGrid = ref(false)

const packingListToolbarShown = computed(() => !!selectedListName.value)

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
