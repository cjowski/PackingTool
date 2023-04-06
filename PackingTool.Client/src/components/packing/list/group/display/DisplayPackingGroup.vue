<template>
  <q-card :class="cardClass">
    <q-card-section
      horizontal
      @click="select($event)"
      @dblclick="editGroup(group.id)"
      class="cursor-pointer"
    >
      <q-item class="col">
        <q-item-section side>
          <q-icon :name="getIconByItemType(group.type)" />
        </q-item-section>

        <q-item-section>
          <q-item-label
            class="text-capitalize text-body1 text-bold non-selectable"
          >
            {{ group.name }}
          </q-item-label>
        </q-item-section>
      </q-item>

      <q-card-actions @click.stop="">
        <div class="col">
          <div class="row justify-end">
            <q-btn icon="more_vert" flat dense color="blue-grey-2" padding="xs">
              <q-menu
                v-model="showMoreButtons"
                anchor="top right"
                transition-show="jump-down"
                transition-hide="jump-up"
                class="transparent no-shadow"
                :offset="[-10, 25]"
              >
                <q-list dense>
                  <q-item>
                    <q-btn
                      icon="edit"
                      @click="editGroup(group.id)"
                      round
                      color="green"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                  <q-item v-if="group.status != ExistenceStatus.New">
                    <q-btn
                      icon="sync"
                      @click="synchronize"
                      round
                      color="orange"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                  <q-item>
                    <q-btn
                      icon="delete"
                      @click="remove"
                      round
                      color="red-5"
                      size="md"
                      padding="sm"
                    />
                  </q-item>
                </q-list>
              </q-menu>
            </q-btn>
          </div>
        </div>
      </q-card-actions>
    </q-card-section>

    <q-separator dark />

    <q-card-section>
      <DisplayPackingItem
        v-for="item in group.items"
        :item="item"
        :groupID="group.id"
      />
    </q-card-section>

    <q-inner-loading :showing="synchronizing">
      <q-spinner-ball size="100px" color="orange" />
    </q-inner-loading>
  </q-card>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { QList } from "quasar"
import DisplayPackingItem from "./DisplayPackingItem.vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import type { PackingGroup } from "@/models/packing/list/PackingGroup"
import getIconByItemType from "@/methods/getIconForItemType"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const { packingListManager } = useAllPackingListsStore()
const { setSelectedGroupID, editGroup } = useOpenedPackingListStore()

const props = defineProps({
  group: {
    type: Object as () => PackingGroup,
    required: true,
  },
  cardClass: {
    type: String,
    required: true,
  },
})

const synchronizing = ref(false)
const showMoreButtons = ref(false)

const select = (event: MouseEvent) => {
  setSelectedGroupID(props.group.id, event.ctrlKey)
}

const synchronize = async () => {
  showMoreButtons.value = false
  synchronizing.value = true
  await packingListManager.SynchronizeGroup(props.group.id)
  synchronizing.value = false
}

const remove = () => {
  showMoreButtons.value = false
  packingListManager.RemoveGroup(props.group.id)
}
</script>
