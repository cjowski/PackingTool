import { nextTick } from "vue"
import { createI18n } from "vue-i18n"

export const defaultLocale = "en"

let _i18n

const setup = (options = { locale: defaultLocale }) => {
  _i18n = createI18n({
    locale: options.locale,
    fallbackLocale: defaultLocale,
  })
  setLocale(options.locale)
  return _i18n
}

const setLocale = (newLocale: string | string[]) => {
  _i18n.global.locale = newLocale
}

const loadMessagesFor = async (locale: string | string[]) => {
  const messages = await import(`../${locale}.json`)
  _i18n.global.setLocaleMessage(locale, messages.default)
  return nextTick()
}

export default {
  get vueI18n() {
    return _i18n
  },
  setup,
  setLocale,
  loadMessagesFor,
}
