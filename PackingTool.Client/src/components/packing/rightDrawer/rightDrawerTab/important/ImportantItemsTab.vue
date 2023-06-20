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
        >
          <q-tooltip>Delete all</q-tooltip>
        </q-btn>
      </q-item-section>
    </q-item>

    <q-separator v-if="importantItems.length > 1" dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <q-item
        v-for="item in importantItems"
        :item="item"
        clickable
        class="important-item-font"
      >
        <q-item-section side>
          <q-checkbox dense v-model="item.packed" color="red" keep-color />
        </q-item-section>

        <q-item-section @click="item.packed = !item.packed">
          {{ item.name }}
        </q-item-section>

        <q-item-section v-if="item.count > 1" side @click.stop="">
          <q-badge rounded outline color="red-2" :label="item.count" />
        </q-item-section>

        <q-item-section side @click.stop="" style="padding-left: 4px">
          <q-btn
            flat
            dense
            icon="close"
            color="red-2"
            @click="removeImportance(item)"
          />
        </q-item-section>
      </q-item>
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
  scrollAreaHeight.value = window.innerHeight - 200
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
  font-size: 15px;
}
</style>
