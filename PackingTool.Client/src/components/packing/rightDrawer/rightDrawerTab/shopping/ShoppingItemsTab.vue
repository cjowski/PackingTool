<template>
  <div>
    <q-item
      v-if="shoppingItems.length > 1"
      clickable
      @click="setAsBoughtAllItems(!allItemsBought)"
    >
      <q-item-section>
        <q-checkbox
          dense
          v-model="allItemsBought"
          color="primary"
          keep-color
          class="shopping-item-all-packed-label"
          >Bought</q-checkbox
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

    <q-separator v-if="shoppingItems.length > 1" dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <q-list>
        <ShoppingItem v-for="item in shoppingItems" :item="item" />
      </q-list>
    </q-scroll-area>

    <q-dialog v-model="showRemoveAllConfirmation" position="right">
      <q-card class="shopping-item-font">
        <q-card-section>
          Do you want to remove all items from shopping list?
        </q-card-section>

        <q-separator dark />

        <q-card-actions align="around">
          <q-btn flat @click="removeAllFromShopping">Yes</q-btn>
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
import ShoppingItem from "./ShoppingItem.vue"

const { shoppingItems } = storeToRefs(useOpenedPackingListStore())

const scrollAreaHeight = ref(0)
const showRemoveAllConfirmation = ref(false)

const allItemsBought = computed({
  get() {
    return shoppingItems.value.every(
      (item) => item.attributes.indexOf("ToBuy") === -1
    )
  },
  set(bought: boolean) {
    setAsBoughtAllItems(bought)
  },
})

const setAsBought = (item: PackingItem, bought: boolean) => {
  if (bought) {
    item.attributes.push("Bought")
    item.RemoveAttribute("ToBuy")
  } else {
    item.attributes.push("ToBuy")
    item.RemoveAttribute("Bought")
  }
}

const setAsBoughtAllItems = (bought: boolean) => {
  shoppingItems.value.forEach((item) => setAsBought(item, bought))
}

const removeFromShopping = (item: PackingItem) => {
  if (item.attributes.indexOf("ToBuy") !== -1) {
    item.RemoveAttribute("ToBuy")
  }
  if (item.attributes.indexOf("Bought") !== -1) {
    item.RemoveAttribute("Bought")
  }
}

const removeAllFromShopping = () => {
  shoppingItems.value.forEach((item) => removeFromShopping(item))
  showRemoveAllConfirmation.value = false
}

onMounted(() => {
  scrollAreaHeight.value = window.innerHeight - 200
})
</script>

<style lang="scss" scoped>
.shopping-item-all-packed-label {
  :deep(.q-checkbox__label) {
    font-family: "Ink Free";
    font-weight: bold;
    font-size: 20px;
  }
}
.shopping-item-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 15px;
}
</style>
