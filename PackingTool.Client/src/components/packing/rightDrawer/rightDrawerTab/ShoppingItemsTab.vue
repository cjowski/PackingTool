<template>
  <div>
    <q-list>
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
          <q-btn flat dense icon="close" @click="removeAllFromShopping" />
        </q-item-section>
      </q-item>

      <q-separator v-if="shoppingItems.length > 1" dark />

      <ShoppingItem v-for="item in shoppingItems" :item="item" />
    </q-list>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue"
import { storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import ShoppingItem from "./ShoppingItem.vue"

const { shoppingItems } = storeToRefs(useOpenedPackingListStore())

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
}
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
