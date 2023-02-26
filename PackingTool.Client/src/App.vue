<template>
  <q-layout view="hHh LpR fFf">
    <q-header elevated height-hint="98" class="z-top">
      <q-toolbar class="toolbar-font">
        <q-btn
          v-if="userAuthorized"
          flat
          @click="drawer = !drawer"
          round
          dense
          icon="menu"
        />
        <q-toolbar-title
          class="text-bold non-selectable cursor-pointer"
          @click="drawer = !drawer"
        >
          Packing lists
        </q-toolbar-title>

        <q-btn to="/login" flat no-caps size="20px"
          ><span class="text-bold">LogIn</span></q-btn
        >
        <q-btn to="/signup" flat no-caps size="20px"
          ><span class="text-bold">SignUp</span></q-btn
        >
        <q-btn v-if="userAuthorized" @click="doLogout" flat no-caps size="20px"
          ><span class="text-bold">LogOut</span></q-btn
        >
      </q-toolbar>
    </q-header>

    <PackingLists v-model="drawer"></PackingLists>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { useQuasar } from "quasar"
import router from "./router"
import { useAuthStore } from "./stores/authStore"
import PackingLists from "./components/packing/sideDrawer/PackingLists.vue"
const { userAuthorized, logout } = useAuthStore()

const $q = useQuasar()
$q.dark.toggle()
const drawer = ref(true)

const doLogout = async () => {
  await logout()
  router.push("/login")
}
</script>

<style scoped lang="scss">
.toolbar-font {
  font-family: "Ink Free";
}
</style>
