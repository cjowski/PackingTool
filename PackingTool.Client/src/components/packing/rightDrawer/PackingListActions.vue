<template>
  <div :style="unhideDrawer ? '' : 'visibility: hidden'">
    <q-drawer
      v-model="packingListActionsShown"
      side="right"
      behavior="desktop"
      bordered
      persistent
      :width="rightDrawerShown ? 362 : 62"
      class="cursor-pointer"
      @click="rightDrawerShown = !rightDrawerShown"
    >
      <q-scroll-area class="fit">
        <q-list class="q-mt-md">
          <q-item class="q-pa-sm">
            <q-btn
              v-if="!packing"
              flat
              dense
              icon="luggage"
              color="green-14"
              size="lg"
              @click="packing = true"
              @click.stop=""
            />
            <q-btn
              v-else
              flat
              dense
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
              dense
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
              flat
              dense
              icon="library_add"
              color="light-green-14"
              size="lg"
              @click="addingGroup = true"
              @click.stop=""
            />
          </q-item>
          <q-item class="q-pa-sm">
            <q-btn
              flat
              dense
              icon="save"
              color="light-blue-13"
              size="lg"
              @click="packingListManager.SavePackingList"
              @click.stop=""
            />
          </q-item>
          <q-item class="q-pa-sm">
            <q-btn
              v-if="!configuringGrid"
              flat
              dense
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
              class="grid-column-count-input"
            ></q-input>
          </q-item>
          <q-item class="q-pa-sm">
            <q-btn
              v-if="packingList.state != PackingListState.New"
              flat
              dense
              icon="sync"
              color="orange"
              size="lg"
              @click="packingListManager.FetchPackingList"
              @click.stop=""
          /></q-item>
        </q-list>
      </q-scroll-area>

      <q-dialog v-model="addingGroup" position="top">
        <AddPackingGroup :closeAddGroupDialog="() => (addingGroup = false)" />
      </q-dialog>
    </q-drawer>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref, watch } from "vue"
import { storeToRefs } from "pinia"
import AddPackingGroup from "@/components/packing/list/AddPackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { PackingListState } from "@/models/packing/list/PackingListState"

const { packingListManager } = useAllPackingListsStore()
const { selectedListName, rightDrawerShown } = storeToRefs(
  useAllPackingListsStore()
)
const { packingList, packing } = storeToRefs(useOpenedPackingListStore())

const addingGroup = ref(false)
const configuringGrid = ref(false)

// const packingListActionsShown = computed(() => !!selectedListName.value)
const unhideDrawer = computed(() => !!selectedListName.value)
const packingListActionsShown = ref(true)

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

<style lang="scss" scoped>
.grid-column-count-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
    height: 40px;
  }
  :deep(input) {
    padding-top: 6px;
  }
}
</style>
