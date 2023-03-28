import { createRouter, createWebHistory } from "vue-router"
import { useAuthenticationStore } from "@/stores/authenticationStore"
import Login from "@/views/user/Login.vue"
import Register from "@/views/user/Register.vue"
import ChangePassword from "@/views/user/ChangePassword.vue"
import PackingList from "@/views/PackingList.vue"
import EmptyPage from "@/views/EmptyPage.vue"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: EmptyPage,
    },
    {
      path: "/list",
      name: "list",
      component: PackingList,
      props: (route) => ({ query: [route.query.name] }),
    },
    {
      path: "/login",
      name: "login",
      component: Login,
    },
    {
      path: "/register",
      name: "register",
      component: Register,
    },
    {
      path: "/changePassword",
      name: "changePassword",
      component: ChangePassword,
    },
  ],
})

router.beforeEach(async (to) => {
  const publicPages = ["/login", "/register"]
  if (publicPages.includes(to.path)) {
    return
  }

  const { isAuthorized } = useAuthenticationStore()
  if (!isAuthorized()) {
    return { name: "login", replace: true }
  }
})

export default router
