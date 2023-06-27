import { createRouter, createWebHistory } from "vue-router"
import i18n from "@/translations/i18n"
import { defaultLocale } from "@/translations/i18n"
import { useAuthenticationStore } from "@/stores/authenticationStore"
import Login from "@/views/user/Login.vue"
import Register from "@/views/user/Register.vue"
import ChangePassword from "@/views/user/ChangePassword.vue"
import PackingList from "@/views/PackingList.vue"
import AdminPanel from "@/views/AdminPanel.vue"
import EmptyPage from "@/views/EmptyPage.vue"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      redirect: `/${defaultLocale}`,
    },
    {
      path: "/:locale",
      children: [
        {
          path: "",
          name: "home",
          component: EmptyPage,
        },
        {
          path: "list",
          name: "list",
          component: PackingList,
          props: (route) => ({ query: [route.query.name] }),
        },
        {
          path: "list",
          name: "list",
          component: PackingList,
          props: (route) => ({ query: [route.query.name] }),
        },
        {
          path: "login",
          name: "login",
          component: Login,
        },
        {
          path: "register",
          name: "register",
          component: Register,
        },
        {
          path: "changePassword",
          name: "changePassword",
          component: ChangePassword,
        },
        {
          path: "admin",
          name: "admin",
          component: AdminPanel,
        },
      ],
    },
  ],
})

router.beforeEach(async (to, from) => {
  const newLocale = to.params.locale
  const prevLocale = from.params.locale

  if (newLocale !== prevLocale) {
    await i18n.loadMessagesFor(newLocale)
    i18n.setLocale(newLocale)
  }

  const publicPages = ["login", "register"]
  if (publicPages.includes(to.name as string)) {
    return
  }

  const { isAuthorized, isAdmin } = useAuthenticationStore()
  if (!isAuthorized()) {
    return { name: "login", params: { locale: newLocale }, replace: true }
  }

  if (to.path == "admin" && !isAdmin()) {
    return { name: "home", params: { locale: newLocale }, replace: true }
  }
})

export default router
