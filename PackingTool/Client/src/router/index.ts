import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/authStore'
import Login from '@/views/user/Login.vue'
import SignUp from '@/views/user/SignUp.vue'
import PackingList from '@/views/PackingList.vue'
import EmptyPage from '@/views/EmptyPage.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: EmptyPage
    },
    {
      path: '/list',
      name: 'list',
      component: PackingList,
      props: route => ( { query: [route.query.name] } )
    },
    {
      path: '/login',
      name: 'login',
      component: Login
    },
    {
      path: '/signup',
      name: 'signup',
      component: SignUp
    }
  ]
})

router.beforeEach(async (to) => {
  const publicPages = ['/login', '/signup']
  const authRequired = !publicPages.includes(to.path)
  const { userAuthorized } = useAuthStore()

  if (authRequired && !userAuthorized) {
    return { name: 'login', replace: true }
  }
})

export default router
