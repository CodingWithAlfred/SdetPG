# C# .NET NUnit Testing Framework with REST API and Playwright UI Automation

[![CI/CD Pipeline](https://github.com/CodingWithAlfred/SdetPG/actions/workflows/GithubActionSdetTest.yml/badge.svg)](https://github.com/CodingWithAlfred/SdetPG/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This repository demonstrates software automation testing using C# .NET and NUnit, featuring both REST API testing and web UI automation with Playwright. The project showcases modern testing practices with CI/CD integration and follows industry-standard patterns such as the Page Object Model (POM).

## 📋 Table of Contents

1. [Project Structure](#project-structure)
2. [Quick Start](#quick-start)
3. [Prerequisites](#prerequisites)
4. [Technology Stack](#technology-stack)
5. [REST API Testing](#rest-api-testing)
6. [UI Automation Testing](#ui-automation-testing)
7. [CI/CD Pipeline](#cicd-pipeline)
8. [Known Issues](#known-issues)
9. [Future Enhancements](#future-enhancements)

## 🗂️ Project Structure

```
SdetPG/
├── RestAPIApp/                           # REST API Testing Framework
│   ├── TestData/                         # JSON test data files
│   ├── Services/                         # API service classes
│   ├── Models/                           # Request/Response data models
│   ├── Tests/                            # NUnit API test classes
├── UIPrometheusJobSearchTest/            # UI Testing Framework
│   ├── Pages/                            # Page Object Model classes
│   ├── Tests/                            # Playwright UI test classes
│   ├── Models/                           # Test configuration models
│   └── Utils/                            # Helper utilities
├── .gitignore                            # Git ignore rules
├── GithubActionSdetTest.yml              # GitHub Actions workflow
└── README.md                             # Project documentation
```

## 🚀 Quick Start

1. **Clone the repository:**
   ```bash
   git clone https://github.com/CodingWithAlfred/SdetPG.git
   cd SdetPG
   ```

2. **Install dependencies:**
   ```bash
   dotnet restore
   ```

3. **Install Playwright browsers:**
   ```bash
   pwsh bin/Debug/net8.0/playwright.ps1 install
   ```

4. **Run all tests:**
   ```bash
   dotnet test
   ```

5. **Run specific test projects:**
   ```bash
   # REST API tests only
   dotnet test RestAPIApp/
   
   # UI tests only
   dotnet test UIPrometheusJobSearchTest/
   ```

[↑ Back to top](#c-net-nunit-testing-framework-with-rest-api-and-playwright-ui-automation)

## 📋 Prerequisites

- **Operating System:** Windows 10/11, macOS, or Linux
- **IDE:** Visual Studio 2022
- **.NET SDK:** Version 8.0 or higher
- **Git:** For version control
- **PowerShell:** Required for Playwright browser installation

## 🛠️ Technology Stack

| Component | Technology |
|-----------|------------|
| **Language** | C# .NET 8+ |
| **Testing Framework** | NUnit 3.x |
| **API Testing** | HttpClient |
| **UI Automation** | Playwright |
| **CI/CD** | GitHub Actions |
| **Reporting** | NUnit XML/HTML Reports |

## 🔌 REST API Testing

The REST API testing framework demonstrates core API validation capabilities:

### Features:
- **HTTP Methods:** GET, POST, PUT, DELETE operations
- **Data-Driven Testing:** JSON-based test data management
- **Response Validation:** Status codes, headers, and body assertions

### Test Coverage:
- ✅ CRUD operations validation
- ✅ Negative testing scenarios
- ✅ Data integrity verification
- ✅ Error response handling

**Status:** ✅ Local Build Passed | ✅ CI Build Passed

## 🎭 UI Automation Testing

The Playwright-based UI testing framework focuses on job search automation workflows:

### Features:
- **Page Object Model:** Maintainable and reusable page components
- **Human-like Interactions:** Reduced bot detection through realistic user behavior
- **Cross-browser Support:** Chromium (additional bot detection implemented here), Firefox, WebKit compatibility
- **Responsive Testing:** Desktop viewport testing

### Test Scenarios:
1. Google search for "Prometheus Group"
2. Navigate to company career page
3. Validate page elements and accordions
4. Search for Senior SDET position

**Status:** ✅ Local Build Passed | ⚠️ CI Build Issues (Google bot detection)

[↑ Back to top](#c-net-nunit-testing-framework-with-rest-api-and-playwright-ui-automation)

## 🔄 CI/CD Pipeline

The project includes a comprehensive GitHub Actions workflow:

### Pipeline Features:
- **Automated Testing:** Runs on every push and pull request
- **Parallel Execution:** REST API and UI tests run in parallel
- **Artifact Collection:** Test reports

### Workflow Triggers:
- Push to `main` branch
- Pull request creation/updates

### Branch Naming Convention:
- `feature/description` - New features
- `bugfix/issue-number` - Bug fixes
- `hotfix/critical-issue` - Critical production fixes

## ⚠️ Known Issues

| Issue | Status | Workaround |
|-------|--------|------------|
| UI tests fail in CI due to Google bot detection | 🔍 Investigating | Run locally |

## 🔮 Future Enhancements

### Short Term (High Priority):
[ ] Implement Playwright stealth mode to bypass Google search bot detection.
[ ] Integrate comprehensive test coverage metrics.
[ ] Enhance error reporting and structured logging.
[ ] Expand bot detection bypass for Firefox and Safari.
[ ] Screenshot Capture: Implement automatic screenshots on test failure.
[ ] Multi-Environment CI: Execute tests across different operating systems.
[ ] Notifications: Integrate Slack or email alerts for build status.

### Long Term (Low Priority):
[ ] Develop a Python version of UI tests with enhanced stealth capabilities.
[ ] Implement a full Continuous Deployment (CD) pipeline.
[ ] Dockerize the framework for consistent containerized test environments.
[ ] Integrate with test management tools (e.g., TestRail).
[ ] Develop test execution dashboards.
[ ] Implement automated test result trend analysis.
[ ] Add flaky test detection and reporting.

---

**📧 Contact:** For questions or support, please open a GitHub issue.

**📄 License:** This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

[↑ Back to top](#c-net-nunit-testing-framework-with-rest-api-and-playwright-ui-automation)