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
          size="lg"
          :disable="smallScreen && rightDrawerShown"
        />
        <q-toolbar-title>
          <span v-if="!smallScreen" class="text-bold non-selectable">{{
            $t("packingLists")
          }}</span>
        </q-toolbar-title>

        <q-btn
          v-if="!isAuthorized()"
          :to="{ name: 'login' }"
          flat
          no-caps
          :size="smallScreen ? '17px' : '20px'"
        >
          <span class="text-bold">{{ $t("logIn") }}</span>
        </q-btn>
        <q-btn
          v-if="!isAuthorized()"
          :to="{ name: 'register' }"
          flat
          no-caps
          :size="smallScreen ? '17px' : '20px'"
        >
          <span class="text-bold">{{ $t("register") }}</span>
        </q-btn>
        <q-btn
          v-if="isAdmin()"
          :to="{ name: 'admin' }"
          flat
          no-caps
          :size="smallScreen ? '17px' : '20px'"
        >
          <span class="text-bold">Admin</span>
        </q-btn>
        <q-btn
          v-if="isAuthorized()"
          :to="{ name: 'changePassword' }"
          flat
          no-caps
          :size="smallScreen ? '17px' : '20px'"
        >
          <span class="text-bold">{{ $t("changePassword") }}</span>
        </q-btn>
        <q-btn
          v-if="isAuthorized()"
          @click="doLogout"
          flat
          no-caps
          :size="smallScreen ? '17px' : '20px'"
        >
          <span class="text-bold">{{ $t("logOut") }}</span>
        </q-btn>
        <LanguageSwitcher />
      </q-toolbar>
    </q-header>

    <PackingLists v-model="packingListsShown"></PackingLists>
    <PackingListActions @click="setRightDrawerFocus" />
    <RightDrawer
      v-model="rightDrawerShown"
      @click="setRightDrawerFocus"
    ></RightDrawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup lang="ts">
import { onMounted } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import router from "./router"
import { useAuthenticationStore } from "./stores/authenticationStore"
import { useAllPackingListsStore } from "./stores/allPackingListsStore"
import { useOperationStatusStore } from "./stores/operationStatusStore"
import LanguageSwitcher from "./components/topToolbar/languageSwitcher.vue"
import PackingLists from "./components/packing/leftDrawer/PackingLists.vue"
import PackingListActions from "./components/packing/rightDrawer/PackingListActions.vue"
import RightDrawer from "./components/packing/rightDrawer/RightDrawer.vue"
import { PackingSectionType } from "./enums/PackingSectionType"
import { computed } from "@vue/reactivity"

const { isAuthorized, enablePackingLists, isAdmin, logout } =
  useAuthenticationStore()
const { setPackingListsShown } = useAllPackingListsStore()
const { packingListsShown, rightDrawerShown } = storeToRefs(
  useAllPackingListsStore()
)
const { currentSectionFocus } = storeToRefs(useOperationStatusStore())

const smallScreen = computed(() => $q.screen.lt.md)

const $q = useQuasar()
$q.dark.toggle()

const doLogout = async () => {
  setPackingListsShown(false)
  await logout()
  router.push("/login")
}

const showPackingLists = () => {
  if (!enablePackingLists()) return false
  setPackingListsShown(!packingListsShown.value)
}

const setRightDrawerFocus = () => {
  currentSectionFocus.value = PackingSectionType.RightDrawer
}

onMounted(() => {
  setPackingListsShown(enablePackingLists())
})
</script>

<style scoped lang="scss">
.toolbar-font {
  font-family: "Ink Free";
}
</style>
