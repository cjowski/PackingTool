<template>
  <div>
    <q-card-section
      horizontal
      @click="select($event)"
      @dblclick="setEditingHeader()"
      class="cursor-pointer"
    >
      <q-item class="col">
        <q-item-section side>
          <q-icon :name="getIconByItemType(group.type)" />
        </q-item-section>

        <q-item-section>
          <q-item-label class="display-group-name non-selectable">
            {{ group.name }}
          </q-item-label>
        </q-item-section>

        <q-item-section side style="padding-left: 0px" @click.stop="">
          <q-btn
            v-if="!editing"
            flat
            dense
            icon="edit"
            @click="editGroup(group.id)"
          />
          <q-btn
            v-else
            flat
            dense
            icon="edit_off"
            @click="finishEditGroup(group.id)"
          />
        </q-item-section>

        <q-item-section side style="padding-left: 0px" @click.stop="">
          <q-btn flat dense icon="close" @click="remove" />
        </q-item-section>
      </q-item>
    </q-card-section>

    <q-expansion-item v-model="editing" header-style="display: none">
      <q-separator dark />

      <q-item v-if="!editingAttributes">
        <q-item-section>
          <q-btn
            flat
            dense
            icon="format_size"
            color="green-14"
            size="md"
            @click="setEditingHeader()"
          />
        </q-item-section>

        <q-item-section v-if="group.status !== ExistenceStatus.New">
          <q-btn
            flat
            dense
            icon="sync"
            color="orange"
            size="md"
            @click="synchronize"
          />
        </q-item-section>

        <q-item-section v-if="group.status !== ExistenceStatus.New">
          <q-btn
            flat
            dense
            icon="dashboard_customize"
            color="cyan-5"
            size="md"
            @click="editAttributesForGroup(group.id)"
          />
        </q-item-section>
      </q-item>

      <q-item v-else class="q-pt-xs q-pb-xs">
        <q-item-section>
          <q-btn
            color="green"
            padding="xs"
            icon="check"
            @click="finishEditAttributesForGroup(group.id)"
          />
        </q-item-section>
      </q-item>
    </q-expansion-item>
  </div>
</template>

<script lang="ts" setup>
import { computed } from "vue"
import { storeToRefs } from "pinia"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import getIconByItemType from "@/methods/getIconForItemType"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const { packingListManager } = useAllPackingListsStore()
const {
  setSelectedGroupID,
  editGroup,
  editAttributesForGroup,
  finishEditGroup,
  finishEditAttributesForGroup,
} = useOpenedPackingListStore()
const { editingGroupIDs, editingAttributesGroupIDs } = storeToRefs(
  useOpenedPackingListStore()
)

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  setEditingHeader: {
    type: Function,
    required: true,
  },
})

const editing = computed(
  () => editingGroupIDs.value.indexOf(props.group.id) !== -1
)

const editingAttributes = computed(
  () => editingAttributesGroupIDs.value.indexOf(props.group.id) !== -1
)

const select = (event: MouseEvent) => {
  setSelectedGroupID(props.group.id, event.ctrlKey)
}

const synchronize = async () => {
  await packingListManager.SynchronizeGroup(props.group.id)
}

const remove = () => {
  packingListManager.RemoveGroup(props.group.id)
}
</script>

<style lang="scss" scoped>
.display-group-name {
  font-size: 18px;
  font-weight: bold;
}
</style>
