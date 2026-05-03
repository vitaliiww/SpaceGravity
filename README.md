# 🪐 SpaceGravity

<img width="859" height="583" alt="SpaceGravity" src="https://github.com/user-attachments/assets/ce858e01-49f3-40f2-80bf-00fc5f5e78f3" />

A lightweight, physically accurate 2D gravity simulation system for Unity. Simulates N-body gravitational interactions with realistic orbital mechanics - planets orbit stars, moons orbit planets, and everything pulls on everything else.

---

## Features

- **N-body gravity** - every body exerts gravitational force on every other body
- **Automatic initial velocities** - bodies are placed on elliptical orbits automatically, no manual tuning needed
- **Hill sphere detection** - determines which body dominates a given region so moons orbit planets instead of flying into the star
- **Configurable eccentricity** - set orbital eccentricity per body (`0` = circle, approaching `1` = very elongated ellipse)
- **Simulation speed control** - run at any time scale (default: 1 day per second)
- **Double-precision math** - custom `Vector2Double` type avoids float errors at astronomical distances
- **Demo scene** - pre-made scene with inner solar system that contains Sun, Mercury, Venus, Earth and Moon with their real masses and distances

---

## Project Structure

```
SpaceGravity/
├── GravityManager.cs   - simulation loop, force integration, initial velocity setup
├── GravityBody.cs      - component attached to each planet/moon/star
├── GravityUtils.cs     - utility functions (Hill sphere calculation)
└── Vector2Double.cs    - double-precision 2D vector struct
```

---

## Quick Start

### 1. Set up the scene

- Create a **GravityManager** GameObject and attach `GravityManager.cs`
- Assign `metersPerUnit`, `g`, and `simulationSpeed` in the Inspector
- Assign your **star** object to the `star` field

### 2. Add bodies

Attach `GravityBody.cs` to each planet or moon. Configure in the Inspector:

| Field | Description |
|---|---|
| `mass` | Mass in kilograms |
| `position` | Starting position in meters (used for physics) |
| `eccentricity` | Orbital shape: `0.0` = circle, `0.9` = very elliptical |

> ⚠️ The `position` field drives physics. The GameObject's `transform.position` is set automatically and scaled by `metersPerUnit`.

### 3. Play

Press Play - initial velocities are calculated automatically and bodies enter stable orbits.

---

## Configuration

All settings live on the `GravityManager` component:

| Field | Default | Description |
|---|---|---|
| `metersPerUnit` | `1e8` | How many real meters one Unity unit represents |
| `g` | `6.674e-11` | Gravitational constant (N·m²/kg²) |
| `simulationSpeed` | `86400` | Seconds of sim time per real second (86400 = 1 day/s) |
| `star` | — | The central star; used as fallback dominant body |

---

## How It Works

### Force Integration (Velocity Verlet-lite)

Each `Update`, the manager:
1. Computes acceleration on every body from all other bodies: `a = G·M / r²`
2. Integrates velocity: `v += a · dt`
3. Integrates position: `p += v · dt`
4. Syncs `transform.position` from physics position scaled by `metersPerUnit`

### Initial Velocity

At `Start`, each body receives a velocity that puts it on a Keplerian ellipse around its **dominant body**:

```
v = sqrt( G·M · (2/r - 1/a) )
```

where `a = r / (1 - e)` is the semi-major axis derived from eccentricity `e`.

### Hill Sphere

The dominant body for a given object is found by checking which body's [Hill sphere](https://en.wikipedia.org/wiki/Hill_sphere) the object falls inside. This ensures moons naturally orbit their planet rather than the star.

---

## Limitations & Notes

- **2D only** - uses `Vector2Double`; z-axis is ignored
- **No collision detection** - bodies pass through each other
- **Euler integration** - simple and fast, but energy is not perfectly conserved over very long simulations. For higher accuracy, consider Leapfrog or RK4 integration
- `position` must be set before Play - it is not synced from `transform.position` on startup


---
 
## Roadmap
 
### Physics
- [ ] **Leapfrog / RK4 integration** — better energy conservation for long-running simulations
- [ ] **3D support** — extend `Vector2Double` to `Vector3Double` and support full 3D orbits
### Simulation
- [ ] **Orbital elements at runtime** — expose semi-major axis, period, apoapsis/periapsis as read-only properties on `GravityBody`
- [ ] **Multi-star systems** — support binary/trinary stars without a single designated star fallback
### Editor & Tooling
- [ ] **Custom Inspector for GravityBody** — visualize Hill sphere and orbit preview directly in the Scene view
- [ ] **Orbit trajectory gizmo** — draw predicted orbit path as a gizmo during Play mode
### Performance
- [ ] **Barnes-Hut tree** — reduce N-body complexity from O(n²) to O(n log n) for large body counts
---

## Dependencies

- Unity (any recent LTS version)
- No external packages required
