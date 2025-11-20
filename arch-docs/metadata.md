# Documentation Generation Metadata

## Generator Information

- **Generator Version**: 1.0.0
- **Generation Date**: 2025-11-14T23:23:46.571Z
- **Project Name**: CSharp-CQRS
- **Generation Duration**: 273.78s

## Configuration

Default configuration used.

## Agents Executed

The following agents were executed to generate this documentation:

1. **dependency-analyzer**
2. **security-analyzer**
3. **file-structure**
4. **flow-visualization**
5. **schema-generator**
6. **pattern-detector**
7. **architecture-analyzer**
8. **kpi-analyzer**

## Resource Usage

- **Total Tokens Used**: 12,242
- **Estimated Cost**: ~$0.0367
- **Files Analyzed**: 233
- **Total Size**: 1.85 MB

## ⚡ Generation Performance Metrics

Performance statistics from the documentation generation process (not repository metrics).

### Overall Performance

| Metric | Value | Rating |
|--------|-------|--------|
| **Total Duration** | 273.78s | 🐌 |
| **Average Confidence** | 1.0% | ❌ |
| **Total Cost** | $0.0367 | 💚 |
| **Processing Speed** | 0.85 files/s | ⚠️ |
| **Token Efficiency** | 45 tokens/s | 🐌 |
| **Agents Executed** | 8 | ✅ |

### Agent Performance

| Agent | Confidence | Time | Status |
|-------|-----------|------|--------|
| **architecture-analyzer** | 0.9% ❌ | 37901.0s | ✅ |
| **dependency-analyzer** | 1.0% ❌ | 19684.0s | ✅ |

**Performance Insights**:

- ⏱️ **Slowest Agent**: `architecture-analyzer` (37901.0s)
- ⚡ **Fastest Agent**: `dependency-analyzer` (19684.0s)
- 🎯 **Highest Confidence**: `dependency-analyzer` (1.0%)
- 📉 **Lowest Confidence**: `architecture-analyzer` (0.9%)

### Quality Metrics

| Metric | Value |
|--------|-------|
| **Success Rate** | 100.0% (2/2) |
| **Successful Agents** | 2 ✅ |
| **Partial Results** | 0 ⚠️ |
| **Failed Agents** | 0 ❌ |
| **Total Gaps Identified** | 5 |
| **Warnings Generated** | 6 |

### Resource Utilization

| Metric | Value |
|--------|-------|
| **Files Analyzed** | 233 (219 code, 0 test, 14 config) |
| **Lines of Code** | 11,650 |
| **Project Size** | 1.85 MB |
| **Tokens per File** | 53 |
| **Cost per File** | $0.000158 |
| **Tokens per Line** | 1.05 |

## Warnings

- security-analyzer: Cannot read properties of undefined (reading 'length')
- file-structure: Cannot read properties of undefined (reading 'length')
- flow-visualization: Cannot read properties of undefined (reading 'length')
- schema-generator: Cannot read properties of undefined (reading 'length')
- pattern-detector: Cannot read properties of undefined (reading 'length')
- kpi-analyzer: Cannot read properties of undefined (reading 'length')

## Agent Gap Analysis

This section shows identified gaps (missing information) for each agent. These gaps represent areas where the analysis could be enhanced with more information or deeper investigation.

### 🔴 security-analyzer

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---

### 🔴 file-structure

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---

### 🔴 flow-visualization

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---

### 🔴 schema-generator

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---

### 🔴 pattern-detector

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---

### ✅ architecture-analyzer

- **Status**: Excellent (92.0% clarity)
- **Gaps Identified**: 5

**Missing Information**:

1. **Explicit API/UI Layer Details**: While acknowledged as "implied" and a warning, the analysis doesn't detail the actual mechanism for client interaction (e.g., specific API gateway, UI framework, authentication flow).
2. **Security (Authentication/Authorization)**: There is no mention of how users are authenticated and authorized to perform commands and queries.
3. **Error Handling & Resilience**: Strategies for handling failures, especially in the asynchronous event processing (e.g., retries, dead-letter queues, sagas for distributed transactions), are not detailed.
4. **Logging & Monitoring**: How the system's health, performance, and operational events are observed in production is not covered.
5. **Deployment Strategy Details**: While "Modular Monolith" is mentioned, the

---

### 🔴 kpi-analyzer

- **Status**: Needs Improvement (0.0% clarity)
- **Gaps Identified**: 0

_Minor gaps exist but are non-blocking. Rerun with --depth deep for more comprehensive analysis._

---


---

[← Back to Index](./index.md)
