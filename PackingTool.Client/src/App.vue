<template>
  <q-layout view="hHh LpR fFf">
    <q-header elevated height-hint="98" class="z-top">
      <q-toolbar>
        <q-btn
          v-if="userAuthorized"
          flat
          @click="drawer = !drawer"
          round
          dense
          icon="menu"
        />
        <q-toolbar-title
          class="text-bold non-selectable cursor-pointer toolbar-font"
          @click="drawer = !drawer"
        >
          Packing lists
        </q-toolbar-title>

        <q-btn flat aria-label="Menu" size="20px" icon="login" to="/login" />
        <q-btn
          flat
          aria-label="Menu"
          size="20px"
          icon="account_circle"
          to="/signup"
        />
        <q-btn
          v-if="userAuthorized"
          @click="doLogout"
          flat
          aria-label="Menu"
          size="20px"
          icon="power_settings_new"
          color="amber-7"
        />
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
