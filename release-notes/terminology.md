# Terminology

This document defines common terms and fields used in .NET release metadata HAL documents.  Most documents follow the HAL (Hypertext Application Language) format, using `_links` and optionally `_embedded` for structure and navigation.

## Top-Level Fields

- **`kind`**: A string describing the type of document (e.g., `index`, `releases`, `manifest`).
- **`description`**: A human-readable summary of what the document contains.
- **`version`**: Indicates the .NET version the document applies to (e.g., `8.0`, `6.0.1xx`).
- **`component`**: Used in SDK/runtime documents to specify the focus (e.g., `sdk`, `runtime`).

## Support Metadata (`support` object)

- **`release-type`**: Indicates whether the release is `lts` (Long-Term Support) or `sts` (Standard-Term Support).
- **`phase`**: Current support phase:
  - `preview`: Pre-release
  - `active`: Actively supported
  - `maintenance`: nearing end of support (some fixes may not be accepted)
  - `eol`: End of life
- **`eol-date`**: ISO 8601 timestamp for when support ends (e.g., `2026-05-12T00:00:00+00:00`).

## Links (`_links`)

- **`self`**: Points to the current document.
- **`index`**: Points to an index document.
- **`releases`**: Points to the associated releases (plural) list for this major version.
- **`release`**: Points to the associated release (singular) list for this patch version.
- **`manifest`**: Points to a manifest file containing high-level metadata (e.g., GA date, EOL date) and contextual links (e.g., announcement blog post).
- **Other links** (e.g., `download`, `release-notes`, `whats-new`): Provide related content and references for users or tools.

Each link includes:

- `href`: Fully qualified URL.
- `relative`: Path relative to the root index (for display or fallback).
- `title`: Optional, used for human-readable labeling.
- `type`: Media type (usually `application/json` or `text/markdown`).

## Embeddings (`_embedded`)

- **`releases`**: An array of child version descriptors (typically `kind: index`) that include their own `version`, `support` metadata, and `_links.self` to navigate to deeper content.

## Flavor

A *flavor* is a specific representation of the same conceptual resource, typically differing by format or context. For example, the `supported-os` content for a release is available in multiple flavors:

- `supported-os` – JSON format (preferred for machine reading)
- `supported-os-markdown` – GitHub-rendered Markdown (for human reading)
- `supported-os-raw-markdown` – raw Markdown (for chat assistants or rendering engines)

Flavors are indicated with consistent naming and differentiated by link `type` (e.g., `application/json`, `text/markdown`, `text/html`). This allows consumers to select the representation that best suits their use case.
