<template>
  <q-item clickable>
    <q-item-section side>
      <q-checkbox
        dense
        v-model="bought"
        color="green"
        keep-color
      />
    </q-item-section>

    <q-item-section @click="setAsBought(!bought)">
      <div class="row q-gutter-xs non-selectable">
        <div class="col-11 q-ma-none">
          <span class="shopping-item-font">{{ item.name }}</span>
        </div>
        <div class="col-1 q-ma-none text-left">
          <span
            class="shopping-item-font"
            :style="item.count == 1 ? 'visibility: hidden' : ''"
          >
            {{ item.count }}
          </span>
        </div>
      </div>
    </q-item-section>

    <q-item-section side @click.stop="">
      <q-btn flat dense icon="close" @click="removeFromShopping()" />
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
  font-weight: bold;
  font-size: 15px;
}
</style>
