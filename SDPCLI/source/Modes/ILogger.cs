using System;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// 日志接口 - 提供统一的日志输出方法
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 普通信息日志
        /// </summary>
        void Info(string message);
        
        /// <summary>
        /// 警告日志
        /// </summary>
        void Warning(string message);
        
        /// <summary>
        /// 错误日志
        /// </summary>
        void Error(string message);
        
        /// <summary>
        /// 调试日志
        /// </summary>
        void Debug(string message);
        
        /// <summary>
        /// 成功日志（带✓标记）
        /// </summary>
        void Success(string message);
    }
}
