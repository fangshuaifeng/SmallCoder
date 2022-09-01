### SmallCoder - 模板代码生成器


本工具基于`Liquid`模板引擎开发，语法详见文档<a href="https://liquid.bootcss.com/" target="_blank" rel="nofollow"> Liquid中文手册</a>。有不明白的地方在下方留言，或参考<a href="https://blog.renzicu.com/2022/small-coder/" target="_blank" rel="nofollow"> SmallCoder在线文档</a>。

> 模板目录：`Templates`，文件后缀`.nxt`。此目录下的第一层文件夹会被视作模板
> 输出目录：`Out`，默认生成文件后缀`.cs`。当模板文件为`*.html.nxt`这样的格式时，生成文件后缀为`.html`

**特别说明**
> `Templates`目录中出现`EntityFolder`文件夹时，会被自动替换成`实体名称`文件夹。
> `Entity.nxt`文件是默认的实体文件，生成时会被替换成`实体名称`文件
> `{{ Entity }}`标签用于展示内容、`{% if %}`标签用于逻辑控制、`raw`标签输出不做解析<span style="color:red;">(注)</span>

![主界面](https://blog.renzicu.com/2022/small-coder/show_1.png)
![模板编辑页面](https://blog.renzicu.com/2022/small-coder/show_2.png)

> 连接数据库（`Mysql`）仅为了动态生成`实体、字段`，其它操作均在本地完成可放心使用。


#### 一、功能清单
|            名称        |                       作用描述                |
|  -------------------  | --------------------------------------------  |
|  数据连接              |     自行点击右侧配置按钮，配置数据连接           |
|  数据库                |     对应具体的数据库 Database                  |
|  数据表                |     对应数据库中的具体的 Table                  |
|  实体名称              |     对Table名称进行二次自定义，模板中实际使用值   |
|  命名空间              |     类的命令空间，模板有文件夹层级时，会自动追加   |
|  功能描述              |     对应功能描述，可用于对Controller的描述        |
|  过滤模板              |     跳过指定规则的模板，示例：Controller;Expand   |

---
#### 二、预置参数
|            名称        |                            作用描述                       |
|  -------------------  | --------------------------------------------------------  |
|  _SpaceName            |     命名空间                                              |
|  _TableName            |     选择的某一个数据库表名                                 |
|  _EntityName           |     实体名称                                              |
|  _Columns              |     表对应的所有列，见 [Columns属性](#三、Columns属性)      |
|  _Description          |     功能描述                                              |
|  _Model                |     自定义JSON参数                                        |

#### 三、Columns属性
|            名称       |                描述                  |      类型    |
|  -------------------  | ----------------------------------  | ------------ |
|  name                 |     列名                             |   string    |
|  comment              |     备注                             |   string    |
|  data_type            |     程序内数据类型                    |   string    |
|  data_type_code       |     数据类型，示例：varchar           |   string    |
|  column_type          |     列类型，示例：varchar(512)        |   string    |
|  char_length          |     字符串长度，示例：512             |    long?    |
|  is_pri               |     是否主键                         |    bool     |
|  number_precision     |     数值长度                         |    long?    |
|  number_scale         |     数值精度                         |    long?    |

#### 四、快捷键说明
|           按键        |         扩展按键        |                              作用描述                          |
|  -------------------  |  --------------------  | -------------------------------------------------------------- |
|  F2                   |                        |     目录：对选中的文件（夹）进行重命名                            |
|  F5                   |   Ctrl + R             |     目录：刷新左侧文件（夹）树                                   |
|  Del                  |   Ctrl + D             |     目录：对选中的文件（夹）进行删除                              |
|  Ctrl + C             |   Ctrl + V             |     目录：复制（粘贴）选中的文件（夹）                            |
|  Ctrl + E             |                        |     目录：打开文件（夹）所在目录                                 |
|  Ctrl + N             |   Ctrl + Shift + N     |     目录：在选中的节点下，创建文件（夹）                          |
|  Ctrl + Shift + C     |                        |     目录：对选中的文件（夹）快速复制粘贴                          |
|  Ctrl + F             |                        |     编辑：在当前编辑的文件中进行高亮搜索                          |
|  Ctrl + S             |                        |     编辑：对已修改的模板文件进行保存                              |
|  ESC                  |                        |     编辑：关闭正在编辑的文件                                     |
|  Alt +  ↑             |   Alt + ↓              |     编辑：在文档中上（下）移动光标所在行的代码                     |
|  Enter                |   Ctrl + Enter         |     查找：搜索框里查找下（上）一个，替换框里替换（全部）下一个      |

---