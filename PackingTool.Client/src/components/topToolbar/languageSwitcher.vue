<template>
  <q-btn round unelevated color="pink-10" class="q-ml-sm" @click="changeLocale">
    <span class="text-bold">{{ i18n.vueI18n.global.locale }}</span>
  </q-btn>
</template>

<script setup lang="ts">
import router from "@/router"
import i18n from "@/translations/i18n"

const locales = ["en", "pl"]

const changeLocale = () => {
  const newLocale =
    i18n.vueI18n.global.locale == locales[0] ? locales[1] : locales[0]
  i18n.setLocale(newLocale)
  router.push({ params: { locale: newLocale } })
}
</script>
