<template>
  <div>
    <q-item
      v-if="importantItems.length > 1"
      clickable
      @click="packAllItems(!allItemsPacked)"
    >
      <q-item-section>
        <q-checkbox
          dense
          v-model="allItemsPacked"
          color="primary"
          keep-color
          class="important-item-all-packed-label"
          >Packed</q-checkbox
        >
      </q-item-section>
      <q-item-section side @click.stop="">
        <q-btn
          flat
          dense
          icon="close"
          color="red"
          @click="showRemoveAllConfirmation = true"
        />
      </q-item-section>
    </q-item>

    <q-separator v-if="importantItems.length > 1" dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <q-list>
        <q-item v-for="item in importantItems" :item="item" clickable>
          <q-item-section side>
            <q-checkbox dense v-model="item.packed" color="red" keep-color />
          </q-item-section>

          <q-item-section @click="item.packed = !item.packed">
            <div class="row q-gutter-xs non-selectable">
              <div class="col-11 q-ma-none">
                <span class="important-item-font">{{ item.name }}</span>
              </div>
              <div class="col-1 q-ma-none text-left">
                <span
                  class="important-item-font"
                  :style="item.count == 1 ? 'visibility: hidden' : ''"
                >
                  {{ item.count }}
                </span>
              </div>
            </div>
          </q-item-section>

          <q-item-section side @click.stop="">
            <q-btn flat dense icon="close" @click="removeImportance(item)" />
          </q-item-section>
        </q-item>
      </q-list>
    </q-scroll-area>

    <q-dialog v-model="showRemoveAllConfirmation" position="right">
      <q-card class="important-item-font">
        <q-card-section>
          Do you want to remove all items from important list?
        </q-card-section>

        <q-separator dark />

        <q-card-actions align="around">
          <q-btn flat @click="removeImportanceForAll">Yes</q-btn>
          <q-btn flat @click="showRemoveAllConfirmation = false">No</q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue"
import { storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import type { PackingItem } from "@/models/packing/list/PackingItem"

const { importantItems } = storeToRefs(useOpenedPackingListStore())

const scrollAreaHeight = ref(0)
const showRemoveAllConfirmation = ref(false)

const allItemsPacked = computed({
  get() {
    return importantItems.value.every((item) => item.packed)
  },
  set(packed: boolean) {
    packAllItems(packed)
  },
})

const packAllItems = (packed: boolean) => {
  importantItems.value.forEach((item) => (item.packed = packed))
}

const removeImportance = (item: PackingItem) => {
  item.RemoveAttribute("Important")
}

const removeImportanceForAll = () => {
  importantItems.value.forEach((item) => item.RemoveAttribute("Important"))
  showRemoveAllConfirmation.value = false
}

onMounted(() => {
  scrollAreaHeight.value = window.innerHeight - 180
})
</script>

<style lang="scss" scoped>
.important-item-all-packed-label {
  :deep(.q-checkbox__label) {
    font-family: "Ink Free";
    font-weight: bold;
    font-size: 20px;
  }
}
.important-item-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 15px;
}
</style>
