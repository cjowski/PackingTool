import { createApp } from "vue"
import { Quasar, Notify, LocalStorage, Loading } from "quasar"
import { createPinia } from "pinia"
import App from "./App.vue"
import router from "./router"
import { initializeApp, type FirebaseOptions } from "firebase/app"
import { firebaseConfig } from "@/config/config.json"
import { OpenAPI } from "./api/core/OpenAPI"

import "./assets/main.css"
import "@quasar/extras/material-icons/material-icons.css"
import "@quasar/extras/animate/bounceIn.css"
import "@quasar/extras/animate/fadeOut.css"
import "quasar/src/css/index.sass"
initializeApp(firebaseConfig as FirebaseOptions)
OpenAPI.BASE = window.location.origin

createApp(App)
  .use(Quasar, { plugins: { Notify, LocalStorage, Loading } })
  .use(createPinia())
  .use(router)
  .mount("#app")
