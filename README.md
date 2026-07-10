# STranslate.Plugin.Translate.Tencent

<div align="center">

**适用于 STranslate 的腾讯云机器翻译插件**

[![GitHub release](https://img.shields.io/github/release/muutot/STranslate.Plugin.Translate.Tencent.svg)](https://github.com/muutot/STranslate.Plugin.Translate.Tencent/releases)
[![Tencent Cloud TMT](https://img.shields.io/badge/TencentCloud-TMT-blue)](https://www.tencentcloud.com/document/product/1161)

本项目为开源翻译软件 [STranslate](https://github.com/STranslate/STranslate) 接入了腾讯云机器翻译 [TMT](https://www.tencentcloud.com/document/product/1161) 服务，让您可以在 STranslate 中无缝体验腾讯云强大的翻译能力。

</div>

---

## ✨ 特性

- 🚀 **无缝集成**：作为 STranslate 的插件，安装后即可在主程序中直接使用。
- 🤖 **官方 API**：基于腾讯云机器翻译 [TMT API 3.0](https://www.tencentcloud.com/document/product/1161) 开发，TC3-HMAC-SHA256 签名认证，稳定可靠。
- 🔒 **安全可靠**：腾讯云提供企业级 SLA 保障，数据安全合规。
- 🌐 **超高性能**：得益于腾讯云全球部署的优质基础设施，提供快速、准确的翻译体验。

---

## 📦 安装

### 手动下载安装

1. 前往本项目的 [Releases](https://github.com/muutot/STranslate.Plugin.Translate.Tencent/releases) 页面。
2. 下载最新版本的 `STranslate.Plugin.Translate.Tencent.spkg` 插件文件。
3. 在 STranslate 的 **"设置"** -> **"插件"** 页面，点击 **"添加插件"**，选择下载的文件。

---

## ⚙️ 配置

安装插件后，您需要配置腾讯云的 SecretId 和 SecretKey 才能使用。

1. 获取腾讯云 API 密钥：
   - 访问 [腾讯云控制台](https://console.cloud.tencent.com/)。
   - 在 **"访问管理"** -> **"API 密钥管理"** 页面创建并获取您的 `SecretId` 和 `SecretKey`。
   - 开通 [机器翻译 TMT](https://console.cloud.tencent.com/tmt) 服务。

2. 在 STranslate 中配置：
   - 在 STranslate 的翻译服务的 **"文本翻译"** 中，点击底部的 **"添加"**，选择刚刚安装的 **Tencent** 插件。
   - 填入您获取的 `SecretId` 和 `SecretKey`。
   - 保存配置后即可开始使用。

---

## 📖 相关文档

- **[STranslate 主项目](https://github.com/STranslate/STranslate)**
- **[腾讯云机器翻译 TMT 文档](https://www.tencentcloud.com/document/product/1161)**
- **[腾讯云 API 密钥管理](https://console.cloud.tencent.com/cam/capi)**

---

## 🤝 贡献

欢迎提交 Issue 来报告 Bug 或提出新功能建议。如果您想贡献代码，欢迎 Fork 本项目并提交 Pull Request。
