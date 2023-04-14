<template></template>

<script setup lang="ts">
import { onMounted } from "vue"
import { useAuthenticationStore } from "@/stores/authenticationStore"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"

const { enablePackingLists } = useAuthenticationStore()
const { setPackingListsShown } = useAllPackingListsStore()

onMounted(() => {
  setPackingListsShown(enablePackingLists())
})
</script>
