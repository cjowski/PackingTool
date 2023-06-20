<template>
  <div :style="unhideDrawer ? '' : 'visibility: hidden'">
    <q-drawer
      v-model="packingListActionsShown"
      side="right"
      behavior="desktop"
      bordered
      persistent
      :overlay="$q.screen.lt.md"
      :width="rightDrawerShown ? 362 : 62"
      class="cursor-pointer"
      @click="toggleRightDrawerShown"
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
              :disable="disableDrawer"
              @click="packing = true"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[65, -35]"
                >Pack items</q-tooltip
              >
            </q-btn>

            <q-btn
              v-else
              flat
              dense
              icon="no_luggage"
              color="deep-orange"
              size="lg"
              :disable="disableDrawer"
              @click="packing = false"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[105, -35]"
                >Cancel packing items</q-tooltip
              >
            </q-btn>
          </q-item>

          <q-item class="q-pa-sm">
            <q-btn
              flat
              dense
              :icon="allGroupsPacked ? 'remove_done' : 'done_all'"
              :color="allGroupsPacked ? 'orange' : 'teal-13'"
              size="lg"
              :style="packing ? '' : 'visibility: hidden;'"
              :disable="disableDrawer"
              @click="packAllGroups(!allGroupsPacked)"
              @click.stop=""
            >
              <q-tooltip
                v-if="allGroupsPacked"
                anchor="bottom left"
                :offset="[85, -35]"
                >Uncheck all items</q-tooltip
              >
              <q-tooltip v-else anchor="bottom left" :offset="[80, -35]"
                >Check all items</q-tooltip
              >
            </q-btn>
          </q-item>

          <q-item class="q-pa-sm">
            <q-btn
              flat
              dense
              icon="library_add"
              color="light-green-6"
              size="lg"
              :disable="disableDrawer"
              :style="packing ? 'visibility: hidden;' : ''"
              @click="addingGroup = true"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[95, -35]"
                >Add packing group</q-tooltip
              >
            </q-btn>
          </q-item>

          <q-item class="q-pa-sm">
            <q-btn
              flat
              dense
              icon="save"
              color="light-blue-13"
              size="lg"
              :disable="disableDrawer"
              @click="packingListManager.SavePackingList"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[55, -35]"
                >Save list</q-tooltip
              >
            </q-btn>
          </q-item>

          <q-item v-if="$q.screen.gt.sm" class="q-pa-sm">
            <q-btn
              v-if="!configuringGrid"
              flat
              dense
              icon="grid_view"
              color="purple-12"
              size="lg"
              :disable="disableDrawer"
              @click="configuringGrid = true"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[90, -35]"
                >Set column count</q-tooltip
              >
            </q-btn>

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
              color="purple-12"
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
              :disable="disableDrawer"
              @click="packingListManager.FetchPackingList"
              @click.stop=""
            >
              <q-tooltip anchor="bottom left" :offset="[60, -35]"
                >Refresh list</q-tooltip
              >
            </q-btn>
          </q-item>
        </q-list>
      </q-scroll-area>

      <q-dialog v-model="addingGroup" position="top">
        <AddPackingGroup :closeAddGroupDialog="() => onAddingGroup()" />
      </q-dialog>
    </q-drawer>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import AddPackingGroup from "@/components/packing/list/AddPackingGroup.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { PackingListState } from "@/models/packing/list/PackingListState"

const $q = useQuasar()

const { packingListManager } = useAllPackingListsStore()
const { selectedListName, packingListsShown, rightDrawerShown } = storeToRefs(
  useAllPackingListsStore()
)
const { packingList, packing } = storeToRefs(useOpenedPackingListStore())

const addingGroup = ref(false)
const configuringGrid = ref(false)
const packingListActionsShown = ref(true)

const unhideDrawer = computed(() => !!selectedListName.value)
const disableDrawer = computed(() => $q.screen.lt.md && packingListsShown.value)

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

const toggleRightDrawerShown = () => {
  if (disableDrawer.value) return
  rightDrawerShown.value = !rightDrawerShown.value
}

const packAllGroups = (value: boolean) => {
  packingList.value.content.groups.forEach((group) =>
    group.items.forEach((item) => (item.packed = value))
  )
}

const onAddingGroup = () => {
  addingGroup.value = false
  setTimeout(() => {
    window.scrollTo({ top: 1000000, behavior: "smooth" })
  }, 50)
}
</script>

<style lang="scss" scoped>
.grid-column-count-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
    height: 40px;
    width: 45px;
  }
  :deep(input) {
    padding-top: 6px;
  }
}
</style>
