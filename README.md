<div align="center">

# ⚔️ Age of The Dead

### Un roguelike de acción y exploración de mazmorras ambientado en un mundo medieval infestado de no-muertos

[![Unity](https://img.shields.io/badge/Motor-Unity-000000?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)
[![C#](https://img.shields.io/badge/Lenguaje-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Estado](https://img.shields.io/badge/Estado-En%20Desarrollo-orange?style=for-the-badge)]()
[![Equipo](https://img.shields.io/badge/Equipo-HERA%20Studio-8B0000?style=for-the-badge)]()

<br/>

> *Desciende a las mazmorras. Sobrevive a los no-muertos. Cada partida es distinta.*

</div>

---

## 📖 Descripción

**Age of The Dead** es un videojuego roguelike 2D de acción desarrollado por **HERA Studio** como proyecto académico del ciclo formativo de Desarrollo de Aplicaciones Multiplataforma (DAM). El juego sitúa al jugador en un entorno medieval donde deberá explorar mazmorras generadas procedimentalmente, combatir hordas de zombis y acumular sinergias de objetos para sobrevivir el mayor tiempo posible.

El proyecto forma parte del ecosistema **PlayAlmi**, una plataforma web que centraliza la gestión de usuarios, clasificaciones globales y estadísticas del juego.

---

## ✨ Características principales

- **Generación procedimental de niveles** — Cada mazmorra se genera de forma aleatoria, garantizando que ninguna partida sea igual a otra.
- **Mecánicas de combate dinámicas** — Sistema de combate ágil con diferentes tipos de enemigos y patrones de ataque.
- **Sistema de sinergias de objetos** — Combinación de objetos y habilidades que crea efectos únicos y builds estratégicas.
- **Dificultad progresiva** — La amenaza escala con el avance del jugador, poniendo a prueba sus reflejos y toma de decisiones.
- **Integración con PlayAlmi** — Sincronización de puntuaciones y estadísticas con la plataforma web del proyecto.

---

## 🛠️ Tecnologías utilizadas

| Tecnología | Uso |
|---|---|
| **Unity** | Motor de videojuego principal |
| **C#** | Lógica de juego y sistemas |
| **Node.js** | API REST del backend (PlayAlmi) |
| **MongoDB** | Base de datos de usuarios y puntuaciones |
| **Azure** | Despliegue del servidor backend |
| **JWT** | Autenticación de usuarios |

---

## 🗂️ Estructura del proyecto

```
PlayAlmi-AgeOfTheDeath/
├── Assets/                 # Recursos del proyecto Unity
│   ├── Scripts/            # Código fuente en C#
│   ├── Scenes/             # Escenas del juego
│   ├── Prefabs/            # Prefabs de personajes, enemigos y entorno
│   ├── Sprites/            # Arte 2D y animaciones
│   └── Audio/              # Efectos de sonido y música
├── Packages/               # Dependencias y paquetes de Unity
├── ProjectSettings/        # Configuración del proyecto Unity
├── .gitignore
├── .gitattributes
└── .vsconfig
```

---

## 🚀 Cómo ejecutar el proyecto

### Requisitos previos

- [Unity Hub](https://unity.com/download) instalado
- Unity Editor **2022.x LTS** o superior
- Visual Studio 2022 o JetBrains Rider (recomendado para C#)

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/HERA-WS/PlayAlmi-AgeOfTheDeath.git
   ```

2. **Abre el proyecto en Unity Hub**
   - Abre Unity Hub → *Add project from disk*
   - Selecciona la carpeta raíz del repositorio

3. **Espera a que Unity importe los Assets**
   - La primera apertura puede tardar varios minutos

4. **Abre la escena principal**
   - Navega a `Assets/Scenes/` y abre la escena de inicio

5. **Pulsa Play** en el editor para probar el juego

---

## 🌐 Plataforma PlayAlmi

Este juego se integra con **[PlayAlmi](https://playalmi.com)**, la plataforma web del proyecto que incluye:

- 🏆 **Clasificación global** con las mejores puntuaciones
- 👤 **Perfiles de usuario** con historial de partidas
- 🌍 **Soporte bilingüe** (Español / Euskera)
- 🔐 **Autenticación JWT** con registro e inicio de sesión

El repositorio del backend y frontend web está disponible en la organización [HERA-WS](https://github.com/HERA-WS).

---

## 👥 Equipo — HERA Studio

Proyecto desarrollado por el equipo **HERA Studio** en el marco del ciclo de Grado Superior de Desarrollo de Aplicaciones Multiplataforma (DAM).

| Miembro | Rol |
|---|---|
| Ekaitz | Desarrollo backend · Integración web · Unity |
| *(resto del equipo)* | *(completar con nombres y roles)* |

---

## 📄 Licencia

Este proyecto ha sido desarrollado con fines académicos. Todos los derechos reservados © HERA Studio.

---

<div align="center">
  <sub>Desarrollado con ❤️ por HERA Studio · DAM · 2024-2025</sub>
</div>
