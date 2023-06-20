<template>
  <q-expansion-item v-model="editing" header-style="display: none">
    <q-separator dark />

    <q-item v-if="!addingItemsGroup && !editingAttributes">
      <q-item-section>
        <q-btn
          flat
          dense
          icon="playlist_add"
          color="green-14"
          size="md"
          @click="addItemsForGroup(group.id)"
        >
          <q-tooltip>Add item</q-tooltip>
        </q-btn>
      </q-item-section>

      <q-item-section>
        <q-btn
          flat
          dense
          icon="format_size"
          color="cyan-5"
          size="md"
          :disable="editingHeader"
          @click="editHeaderForGroup(group.id)"
        >
          <q-tooltip>Edit group details</q-tooltip>
        </q-btn>
      </q-item-section>

      <q-item-section v-if="!isNew">
        <q-btn
          flat
          dense
          icon="sync"
          color="orange"
          size="md"
          @click="synchronize"
        >
          <q-tooltip>Refresh group</q-tooltip>
        </q-btn>
      </q-item-section>

      <q-item-section>
        <q-btn
          flat
          dense
          icon="dashboard_customize"
          color="pink-5"
          size="md"
          @click="editAttributesForGroup(group.id)"
        >
          <q-tooltip>Mark items</q-tooltip>
        </q-btn>
      </q-item-section>
    </q-item>

    <AddPackingItem
      v-else-if="addingItemsGroup"
      :groupID="props.group.id"
      :isNew="isNew"
    />

    <q-item v-else-if="editingAttributes" class="q-pt-xs q-pb-xs">
      <q-item-section>
        <q-btn
          color="green"
          padding="xs"
          icon="check"
          @click="finishEditAttributesForGroup(group.id)"
        />
      </q-item-section>

      <q-item-section v-if="group.status !== ExistenceStatus.New">
        <q-btn color="orange" padding="xs" icon="sync" @click="synchronize" />
      </q-item-section>
    </q-item>
  </q-expansion-item>
</template>

<script lang="ts" setup>
import { computed } from "vue"
import { storeToRefs } from "pinia"
import AddPackingItem from "./AddPackingItem.vue"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingListManager } = useAllPackingListsStore()
const {
  editHeaderForGroup,
  editAttributesForGroup,
  addItemsForGroup,
  finishEditAttributesForGroup,
} = useOpenedPackingListStore()
const {
  editingGroupIDs,
  editingHeaderGroupIDs,
  editingAttributesGroupIDs,
  addingItemsGroupIDs,
} = storeToRefs(useOpenedPackingListStore())

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
})

const isNew = computed(() => props.group.status === ExistenceStatus.New)

const editing = computed(
  () => editingGroupIDs.value.indexOf(props.group.id) !== -1
)

const addingItemsGroup = computed(
  () => addingItemsGroupIDs.value.indexOf(props.group.id) !== -1
)

const editingHeader = computed(
  () => editingHeaderGroupIDs.value.indexOf(props.group.id) !== -1
)

const editingAttributes = computed(
  () => editingAttributesGroupIDs.value.indexOf(props.group.id) !== -1
)

const synchronize = async () => {
  await packingListManager.SynchronizeGroup(props.group.id)
}
</script>
