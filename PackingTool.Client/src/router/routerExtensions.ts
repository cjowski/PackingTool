import router from "@/router"
import i18n from "@/translations/i18n"

const pushWithLocale = (name: string, query?: any) => {
  if (query) {
    router.push({
      name: name,
      query: query,
      params: { locale: i18n.vueI18n.global.locale },
    })
  } else {
    router.push({
      name: name,
      params: { locale: i18n.vueI18n.global.locale },
    })
  }
}

export default { pushWithLocale }
