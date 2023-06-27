import { createApp } from "vue"
import { Quasar, Notify, LocalStorage, Loading } from "quasar"
import { createPinia } from "pinia"
import i18n from './translations/i18n/index'
import App from "./App.vue"
import router from "./router"
import { OpenAPI } from "./api/core/OpenAPI"

import "./assets/main.css"
import "@quasar/extras/material-icons/material-icons.css"
import "@quasar/extras/animate/bounceIn.css"
import "@quasar/extras/animate/fadeOut.css"
// import iconSet from "quasar/icon-set/ionicons-v4";
// import "@quasar/extras/ionicons-v4/ionicons-v4.css";
// import iconSet from 'quasar/icon-set/fontawesome-v6'
// import '@quasar/extras/fontawesome-v6/fontawesome-v6.css'
import "quasar/src/css/index.sass"

OpenAPI.BASE = window.location.origin
i18n.setup()

createApp(App)
  .use(Quasar, { plugins: { Notify, LocalStorage, Loading } })
  .use(createPinia())
  .use(router)
  .use(i18n.vueI18n)
  .mount("#app")
