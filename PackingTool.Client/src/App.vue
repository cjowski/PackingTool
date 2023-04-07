<template>
  <q-layout view="hHh LpR lFr">
    <q-header elevated height-hint="98" class="z-top">
      <q-toolbar class="toolbar-font">
        <q-btn
          v-if="enablePackingLists()"
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
        <q-btn v-if="isAdmin()" to="/admin" flat no-caps size="20px"
          ><span class="text-bold">Admin</span></q-btn
        >
        <q-btn
          v-if="isAuthorized()"
          to="/changePassword"
          flat
          no-caps
          size="20px"
          ><span class="text-bold">Change Password</span></q-btn
        >
        <q-btn v-if="isAuthorized()" @click="doLogout" flat no-caps size="20px"
          ><span class="text-bold">LogOut</span></q-btn
        >
      </q-toolbar>
    </q-header>

    <PackingLists v-model="packingListsShown"></PackingLists>
    <PackingListActions />
    <RightDrawer v-model="rightDrawerShown"></RightDrawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { onMounted, watch } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import router from "./router"
import { useAuthenticationStore } from "./stores/authenticationStore"
import { useAllPackingListsStore } from "./stores/allPackingListsStore"
import PackingLists from "./components/packing/leftDrawer/PackingLists.vue"
import PackingListActions from "./components/packing/rightDrawer/PackingListActions.vue"
import RightDrawer from "./components/packing/rightDrawer/RightDrawer.vue"
import { useRoute } from "vue-router"

const { isAuthorized, enablePackingLists, isAdmin, logout } =
  useAuthenticationStore()
const { packingListsShown, rightDrawerShown } = storeToRefs(useAllPackingListsStore())

const route = useRoute()
const $q = useQuasar()
$q.dark.toggle()

const doLogout = async () => {
  await logout()
  router.push("/login")
}

const showPackingLists = () => {
  if (!enablePackingLists()) {
    return false
  }

  packingListsShown.value = !packingListsShown.value
}

watch(
  route,
  () => {
    packingListsShown.value = enablePackingLists()
  },
  { flush: "pre", immediate: true, deep: true }
)

onMounted(() => {
  packingListsShown.value = enablePackingLists()
})
</script>

<style scoped lang="scss">
.toolbar-font {
  font-family: "Ink Free";
}
</style>
