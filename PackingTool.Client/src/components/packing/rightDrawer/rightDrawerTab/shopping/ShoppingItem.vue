<template>
  <q-item clickable class="shopping-item-font">
    <q-item-section side>
      <q-checkbox dense v-model="bought" color="green" keep-color />
    </q-item-section>

    <q-item-section @click="setAsBought(!bought)">
      {{ item.name }}
    </q-item-section>

    <q-item-section v-if="item.count > 1" side @click.stop="">
      <q-badge rounded outline color="green-2" :label="item.count" />
    </q-item-section>

    <q-item-section side @click.stop="" style="padding-left: 4px">
      <q-btn flat dense icon="close" color="green-2" @click="removeFromShopping()" />
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
})

const bought = computed({
  get() {
    return props.item.attributes.indexOf("Bought") !== -1
  },
  set(bought: boolean) {
    setAsBought(bought)
  },
})

const setAsBought = (bought: boolean) => {
  if (bought) {
    props.item.attributes.push("Bought")
    props.item.RemoveAttribute("ToBuy")
  } else {
    props.item.attributes.push("ToBuy")
    props.item.RemoveAttribute("Bought")
  }
}

const removeFromShopping = () => {
  if (props.item.attributes.indexOf("ToBuy") !== -1) {
    props.item.RemoveAttribute("ToBuy")
  }
  if (props.item.attributes.indexOf("Bought") !== -1) {
    props.item.RemoveAttribute("Bought")
  }
}
</script>

<style lang="scss" scoped>
.shopping-item-font {
  font-family: "Segoe Print";
  font-size: 15px;
}
</style>
