using System;

namespace SnapdragonProfilerCLI.Modes
{
    /// <summary>
    /// 运行模式接口 - 定义所有模式的通用契约
    /// </summary>
    public interface IMode
    {
        /// <summary>
        /// 模式名称
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// 模式描述
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// 执行模式的主逻辑
        /// </summary>
        void Run();
    }
}
