# AI Assistant Guide for .NET Release Notes

This repository contains .NET release metadata in a structured, machine-readable format designed for AI assistants and automated tooling. Raw URLs are provided to access to the files in case readers do not have direct access to the repo.

## Quick Start

**For AI assistants working with .NET release data:**

1. **Start with the JSON API**: [`index.json`](https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json) 
   - Machine-readable entry point with navigation links
   - Follow HAL `_links` for discovery and traversal

2. **Understand the terminology**: [`terminology.md`](terminology.md)
   - Definitions of fields, abbreviations, and document types
   - Essential for interpreting JSON data correctly

3. **Learn the workflows**: [`usage.md`](usage.md)
   - Detailed examples and patterns for common queries
   - Assistant-specific guidance and best practices

## Document Types

- **JSON files** (`.json`): Authoritative, structured data - use these primarily
- **Markdown files** (`.md`): Human-readable documentation and fallback content

## Base URL

All relative paths are based on: `https://raw.githubusercontent.com/richlander/core/main/release-notes`
