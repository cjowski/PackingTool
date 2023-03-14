<template>
  <q-layout view="hHh LpR fFf">
    <q-header elevated height-hint="98" class="z-top">
      <q-toolbar class="toolbar-font">
        <q-btn
          v-if="isAuthorized()"
          flat
          @click="showPackingLists"
          round
          dense
          icon="menu"
        />
        <q-toolbar-title
          class="text-bold non-selectable cursor-pointer"
          @click="showPackingLists"
        >
          Packing lists
        </q-toolbar-title>

        <q-btn v-if="!isAuthorized()" to="/login" flat no-caps size="20px"
          ><span class="text-bold">LogIn</span></q-btn
        >
        <q-btn v-if="!isAuthorized()" to="/register" flat no-caps size="20px"
          ><span class="text-bold">Register</span></q-btn
        >
        <q-btn v-if="isAuthorized()" @click="doLogout" flat no-caps size="20px"
          ><span class="text-bold">LogOut</span></q-btn
        >
      </q-toolbar>
    </q-header>

    <PackingLists v-model="packingListsShown"></PackingLists>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref, watch } from "vue"
import { useQuasar } from "quasar"
import router from "./router"
import { useAuthenticationStore } from "./stores/authenticationStore"
import PackingLists from "./components/packing/sideDrawer/PackingLists.vue"
import { useRoute } from "vue-router"
const { isAuthorized, logout } = useAuthenticationStore()

const route = useRoute()
const $q = useQuasar()
$q.dark.toggle()
const packingListsShown = ref(isAuthorized())

const doLogout = async () => {
  await logout()
  router.push("/login")
}

const showPackingLists = () => {
  if (!isAuthorized()) {
    return false
  }

  packingListsShown.value = !packingListsShown.value
}

watch(
  route,
  () => {
    packingListsShown.value = isAuthorized()
  },
  { flush: "pre", immediate: true, deep: true }
)
</script>

<style scoped lang="scss">
.toolbar-font {
  font-family: "Ink Free";
}
</style>
