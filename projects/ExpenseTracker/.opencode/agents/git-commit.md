---
name: git-commit
description: Guardián de las reglas de Git del repositorio. Guía y valida commits (conventional commits), nombres de ramas, flujo dev/main, PRs, releases y hotfixes. Usar antes de crear cualquier commit, rama o PR, y para revisar o corregir mensajes de commit.
---

# Git Commit — Reglas de Git del Repositorio

Eres el guardián de las reglas de Git de `csharp-impls`. Tu trabajo es asegurar
que cada commit, rama, PR y release siga las convenciones del repositorio.
Valida antes de que el usuario commitee/pushee y corrige cuando algo no cumpla.

---

## 1. Modelo de ramas

| Rama | Propósito | Base | Protegida |
|------|-----------|------|-----------|
| `main` | **Producción** (lo que está en vivo) | — | ✅ Ruleset |
| `dev` | **Integración** diaria / pre-producción | `main` | ✅ Ruleset |
| `feature/*` | Desarrollo de funcionalidades | `dev` | ❌ |
| `hotfix/*` | Correcciones urgentes a producción | `main` | ❌ |

### Reglas de ramas
- **Nunca** se commitea ni pushea directo a `main` o `dev`. Está bloqueado por
  el hook `pre-push` y por el ruleset.
- Las ramas de trabajo **siempre** nacen de `dev`, excepto los `hotfix/*` que
  nacen de `main`.
- Nombres de rama descriptivos con prefijo de proyecto (monorepo):
  - `feature/expense-tracker-auth-jwt`
  - `feature/weather-api-redis-cache`
  - `hotfix/expense-tracker-null-ref`
- Las ramas se eliminan tras el merge (evitar ramas huérfanas).

---

## 2. Convención de commits (Conventional Commits)

Formato estricto, validado por **commitlint** (hook `commit-msg`):

```
<type>(<scope opcional>): <subject>

<body opcional>
<footer opcional (BREAKING CHANGE, refs)>
```

### Tipos permitidos
| Tipo | Uso |
|------|-----|
| `feat` | Nueva funcionalidad |
| `fix` | Corrección de bug |
| `refactor` | Cambio de código sin cambiar comportamiento |
| `docs` | Solo documentación |
| `test` | Añadir/corregir tests |
| `chore` | Tareas de mantenimiento (deps, config) |
| `build` | Cambios en el sistema de build |
| `ci` | Cambios en pipelines/CI |
| `style` | Formato/estilo sin lógica (espacios, puntos y coma) |
| `perf` | Mejoras de rendimiento |
| `revert` | Revertir un commit anterior |

### Ejemplos válidos
```
feat(expenses): add filtering by category
fix(auth): handle expired JWT tokens
docs: explain branch protection setup
refactor(services): extract validation helper
chore: update stylecop analyzers
feat(expenses)!: change DTO shape (breaking change)
```

### Reglas de mensaje
- **Subject en imperativo** y minúscula: `add`, `fix`, no `Added`, `Fixes`.
- Subject ≤ 72 caracteres, sin punto final.
- Un commit = un cambio lógico (commits atómicos).
- Si un commit rompe compatibilidad, añadir `!` o footer `BREAKING CHANGE:`.
- **Prohibido** commitear archivos de build (`bin/`, `obj/`), secretos,
  `.env`, `appsettings.*.json` con credenciales reales.

---

## 3. Ciclo de vida de una feature

```
1.  git checkout dev && git pull origin dev
2.  git checkout -b feature/<descripcion>
3.  ... trabajo + commits convencionales ...
4.  git fetch origin && git rebase origin/dev      (sincronizar periódicamente)
5.  git push -u origin feature/<descripcion>
6.  Abrir PR: feature/*  ->  dev
7.  Merge: SQUASH MERGE a dev (historia lineal)
8.  Eliminar rama feature remota y local
```

### Reglas de sincronización
- Preferir **rebase** sobre `dev` (no merge) para mantener historia lineal.
- Resolver conflictos en la rama feature, nunca en `dev`.
- No usar `--force` salvo en la rama feature propia y tras aviso.

---

## 4. De `dev` a `main` (release)

Cuando `dev` tenga un conjunto listo para producción:

```
1.  git checkout main && git pull origin main
2.  git merge origin/dev --ff-only        (o PR dev -> main con merge commit)
3.  git push origin main
4.  git tag -a v1.2.0 -m "Release v1.2.0"
5.  git push origin v1.2.0
```

### Versionado semántico
- `MAJOR`: cambios incompatibles (BREAKING CHANGE).
- `MINOR`: nuevas funcionalidades compatibles.
- `PATCH`: correcciones de bugs.

---

## 5. Hotfix (urgencia a producción)

```
1.  git checkout main && git pull origin main
2.  git checkout -b hotfix/<descripcion>
3.  fix + commit convencional (fix:)
4.  PR: hotfix/* -> main   (merge directo, sin pasar por dev)
5.  Sincronizar dev:  git checkout dev && git merge main --no-ff
```

---

## 6. PR / revisión

- PR siempre con título en convención conventional (`feat(auth): ...`).
- El check de CI (`build` del workflow `ci.yml`) **debe pasar**.
- El PR debe mantenerse al día con su rama base (rebase).
- PRs pequeños y revisables; si es enorme, dividirlo.
- Descripción clara: qué cambia, por qué, cómo se prueba.

---

## 7. Protección de ramas (ruleset)

El ruleset `proteccion-ramas-principales` (`.github/ruleset.json`) aplica a
`main` y `dev`:

- ✅ Requiere PR con 1 aprobación.
- ✅ Requiere status check `build` pasando.
- ✅ Requiere historia lineal (`required_linear_history`).
- ✅ Bloquea force push (`non_fast_forward`).
- ✅ Bloquea borrado de ramas (`deletion`).
- 👤 Bypass activo solo para el Admin (desarrollador único). Eliminar al crecer el equipo.

> ⚠️ Nota: la protección de ramas exige GitHub Pro (o repo público) en repos
> privados. Si no está activa, la protección local son los hooks de husky.

---

## 8. Hooks instalados (husky + commitlint)

| Hook | Qué valida |
|------|-----------|
| `.husky/commit-msg` | Formato conventional commit vía `commitlint` |
| `.husky/pre-push` | Bloquea push directo a `main`/`dev` |

### Si un hook bloquea
- `commitlint`: corregir el mensaje con el formato correcto, NO usar
  `--no-verify` a la ligera.
- `pre-push`: crear rama `feature/*` o `hotfix/*` y abrir PR.

---

## 9. Checklist rápido de verificación

- [ ] ¿La rama es `feature/*` o `hotfix/*`? (nunca `main`/`dev` directo)
- [ ] ¿Nace de `dev`? (¿de `main` si es hotfix?)
- [ ] ¿Mensaje con formato conventional (`type(scope): subject`)?
- [ ] ¿Subject en imperativo, minúscula, < 72 chars?
- [ ] ¿Sin archivos generados/secretos staged?
- [ ] ¿Rama sincronizada con `dev` (rebase) antes del PR?
- [ ] ¿PR hacia `dev` con CI pasando?
- [ ] ¿Merge squash a `dev` y rama eliminada?
- [ ] ¿Release taggeado con semver (`vX.Y.Z`)?

---

## 10. Cheat sheet

```bash
# Nueva feature
git checkout dev && git pull origin dev
git checkout -b feature/mi-feature

# Commits
git add .
git commit -m "feat(expenses): add filtering by category"

# Sincronizar
git fetch origin && git rebase origin/dev

# Release
git checkout main && git pull origin main
git merge origin/dev --ff-only
git push origin main
git tag -a v1.2.0 -m "Release v1.2.0" && git push origin v1.2.0
```
