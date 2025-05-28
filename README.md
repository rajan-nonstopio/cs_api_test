# API Testing Tool

This is a .NET tool designed for comparing API responses between two different environments (e.g., production vs. staging, or before/after a deployment). It allows you to define test cases, run them against two API endpoints simultaneously, and compare the results.

## Table of Contents
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
  - [Running the Tool](#running-the-tool)
- [Adding Test Cases](#adding-test-cases)
- [Reviewing Results](#reviewing-results)
- [Extending the Tool](#extending-the-tool)

## Features

- Run the same API requests against two different environments
- Compare response times, status codes, and response content
- Export results to Excel for easy analysis
- Configurable request parameters and headers
- Support for various HTTP methods (GET, POST, PUT, etc.)

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Microsoft Excel (for viewing exported results)

### Configuration

1. Copy `appsettings.example.json` to `appsettings.json`
2. Update the following settings in `appsettings.json`: