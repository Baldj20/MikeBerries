import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import {keycloakify} from "keycloakify/vite-plugin";

export default defineConfig({
  plugins: [
      react(),
      keycloakify({
          accountThemeImplementation: "none",
          startKeycloakOptions:{
              port: 8888,
              realmJsonFilePath: "mikeberries_realm.json",
              dockerExtraArgs: [
                  "--name", "Auth"
              ]
          }
      })
  ],
    base: '/products',
})
